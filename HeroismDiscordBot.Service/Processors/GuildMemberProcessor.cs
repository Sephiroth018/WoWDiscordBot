using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;
using HeroismDiscordBot.Service.Logging;
using MoreLinq;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = WowDotNetAPI.Models.Character;

namespace HeroismDiscordBot.Service.Processors
{
    public class GuildMemberProcessor : IProcessor
    {
        private const string MemberJoinedTitle = "Neuzugang! Willkommen!";
        private const string MemberLeftTitle = "Gildenmitglied hat uns verlassen!";
        private readonly IConfiguration _configuration;
        private readonly IDiscordFactory _discordclientFactory;
        private readonly Container _container;
        private readonly ILogger _logger;
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IWoWFactory _wowClientFactory;
        private Timer _timer;

        public GuildMemberProcessor(IConfiguration configuration,
                                    Func<IRepository> repositoryFactory,
                                    IWoWFactory wowClientFactory,
                                    IDiscordFactory discordclientFactory,
                                    Container container,
                                    ILogger logger)
        {
            _configuration = configuration;
            _repositoryFactory = repositoryFactory;
            _wowClientFactory = wowClientFactory;
            _discordclientFactory = discordclientFactory;
            _container = container;
            _logger = logger;
        }

        public void Start()
        {
            _timer = new Timer();
            _timer.Elapsed += (sender, args) => DoWork();
            _timer.Interval = new TimeSpan(1, 0, 0).TotalMilliseconds;
            _timer.AutoReset = true;
            _timer.Start();
            Task.Factory.StartNew(DoWork);
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void DoWork()
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                try
                {
                    using (var repository = _repositoryFactory.Invoke())
                    {
                        var guildMembers = _wowClientFactory.GetClient()
                                                            .GetGuild(_configuration.WoWRegion, _configuration.WoWRealm, _configuration.WoWGuild, GuildOptions.GetEverything)
                                                            .Members
                                                            .ToList();
                        var guildMembersWithState = GetGuildCharacters(repository, guildMembers)
                                                    .AsParallel()
                                                    .WithDegreeOfParallelism(5)
                                                    .Select(GetWoWCharacterData)
                                                    .Select(GetGuildMemberState)
                                                    .ToList();

                        var characters = guildMembersWithState.Where(data => data.state == GuildMemberState.Left)
                                                              .Select(data =>
                                                                      {
                                                                          data.character.Left = DateTime.Now;
                                                                          return data.character;
                                                                      })
                                                              .ToList();

                        characters.AddRange(guildMembersWithState.Where(data => data.state == GuildMemberState.Joined)
                                                                 .Select(data => CreateNewCharacter(repository, data))
                                                                 .Select(EnrichCharacter));

                        characters.AddRange(guildMembersWithState.Where(data => data.state == GuildMemberState.Changed)
                                                                 .Select(EnrichCharacter));

                        repository.SaveChanges();

                        characters.Where(c => c.Player == null)
                                  .ForEach(c =>
                                           {
                                               var existingCharacter = repository.Characters
                                                                                 .ToList()
                                                                                 .Union(repository.Characters.Local)
                                                                                 .FirstOrDefault(ec => ec.AchievementPoints == c.AchievementPoints
                                                                                                       && ec.AchievementsHash == c.AchievementsHash
                                                                                                       && ec.PetsHash == c.PetsHash);

                                               c.Player = existingCharacter?.Player ?? repository.Players.Create();
                                           });

                        repository.SaveChanges();

                        characters.Select(c => c.Player)
                                  .Distinct()
                                  .ForEach(player =>
                                           {
                                               var main = player
                                                          .Characters
                                                          .GroupBy(c => c.Rank)
                                                          .OrderBy(c => c.Key)
                                                          .ToList();

                                               player.Characters.ForEach(c => c.IsMain = false);

                                               if (main.First().Count() == 1)
                                                   main.First()
                                                       .First()
                                                       .IsMain = true;
                                           });

                        repository.SaveChanges();

                        characters.Union(characters.SelectMany(c => c.Player.Characters))
                                  .DistinctBy(c => c.Name)
                                  .Select(data => CreateDiscordMessage(repository, data))
                                  .ForEach(SendDiscordMessage);

                        repository.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    _logger.LogException(e);
                }
            }
        }

        private (GuildMember, Character, Entities.Character character, GuildMemberState state) GetGuildMemberState((GuildMember, Character, Entities.Character) data)
        {
            var (guildMember, characterInfo, character) = data;
            var state = GuildMemberState.Unchanged;
            if (guildMember == null)
                state = GuildMemberState.Left;
            else if (character == null)
                state = GuildMemberState.Joined;
            else if (character.Level != characterInfo.Level
                     || character.Rank != guildMember.Rank
                     || character.Specializations.All(s => s.Name != guildMember.Character.Specialization?.Name || s.Name == guildMember.Character.Specialization?.Name && s.ItemLevel != characterInfo.Items.AverageItemLevelEquipped))
                state = GuildMemberState.Changed;

            return (guildMember, characterInfo, character, state);
        }

        private (GuildMember, Character, Entities.Character) GetWoWCharacterData((GuildMember guildMember, Entities.Character character) data)
        {
            var (guildMember, character) = data;
            var characterInfo = _wowClientFactory.GetClient().GetCharacter(_configuration.WoWRegion, _configuration.WoWRealm, guildMember.Character.Name, CharacterOptions.GetEverything);

            return (guildMember, characterInfo, character);
        }

        private void SendDiscordMessage((CharacterDiscordMessage message, Embed messageData) data)
        {
            var channel = _discordclientFactory.GetGuild()
                                               .GetTextChannelAsync(_configuration.DiscordMemberChangeChannelId)
                                               .Result;

            if (data.message.Id == default(int))
            {
                var message = channel.SendMessageAsync("", embed: data.messageData)
                                     .Result;
                data.message.MessageId = (long)message.Id;
                data.message.ChannelId = (long)channel.Id;
            }
            else
            {
                var sentMessage = channel.GetMessageAsync((ulong)data.message.MessageId)
                                         .Result as IUserMessage;
                sentMessage.ModifyAsync(m => m.Embed = data.messageData).Wait();
            }
        }

        private (CharacterDiscordMessage, Embed) CreateDiscordMessage(IRepository repository, Entities.Character character)
        {
            CharacterDiscordMessage message;
            Embed messageData;

            if (character.Left.HasValue && character.DiscordMessages.All(m => m.DiscordMessageType != DiscordMessageType.Left))
            {
                message = repository.CharacterDiscordMessages.Create();
                message.DiscordMessageType = DiscordMessageType.Left;
                message.Character = character;

                repository.CharacterDiscordMessages.Add(message);
                messageData = BuildPlayerChangedMessage(character, MemberLeftTitle, character.Left.Value, Color.Red, GetAlts(character));
            }
            else if (character.DiscordMessages.All(m => m.DiscordMessageType != DiscordMessageType.Joined))
            {
                message = repository.CharacterDiscordMessages.Create();
                message.DiscordMessageType = DiscordMessageType.Joined;
                message.Character = character;

                repository.CharacterDiscordMessages.Add(message);
                messageData = BuildPlayerChangedMessage(character, MemberJoinedTitle, character.Joined, Color.Green, GetAlts(character));
            }
            else
            {
                message = character.DiscordMessages.First(m => m.DiscordMessageType == DiscordMessageType.Joined);
                messageData = BuildPlayerChangedMessage(character, MemberJoinedTitle, character.Joined, Color.Green, GetAlts(character));
            }

            return (message, messageData);
        }

        private static Embed BuildPlayerChangedMessage(Entities.Character character, string title, DateTime timestamp, Color color, IReadOnlyCollection<Entities.Character> alts)
        {
            var embed = new EmbedBuilder { Title = title };

            embed.WithColor(color);

            embed.AddInlineField("Wer", character.Name);
            embed.AddInlineField("Wann", timestamp.ToString("g"));
            embed.AddInlineField("Klasse", character.Class);

            if (alts.Any())
                embed.AddField("Alt(s)", string.Join(Environment.NewLine, alts.Select(c => $"{(c.IsMain ? "**Main: **" : string.Empty)}{c.Name} - {string.Join(", ", c.Specializations.Select(s => s.Role))}")));

            if (character.Specializations.Any())
                embed.AddField("Spec(s)", string.Join(Environment.NewLine, character.Specializations.Select(s => $"{s.Role} - {s.Name} ({s.ItemLevel})")));

            return embed.Build();
        }

        private static List<Entities.Character> GetAlts(Entities.Character c)
        {
            return c.Player.Characters.Where(a => a.Name != c.Name && !a.Left.HasValue)
                    .OrderBy(a => !a.IsMain)
                    .ToList();
        }

        private static string GeneratePetsHash(CharacterPets pets)
        {
            var petsStringCollection = pets.Collected
                                           .OrderBy(p => p.BattlePetId)
                                           .ThenBy(p => p.QualityId)
                                           .Select(p => $"{p.Name}{p.BattlePetId}{p.QualityId}")
                                           .ToList();

            return string.Join(";", petsStringCollection)
                         .CalculateMD5Hash();
        }

        private static string GenerateAchievementsHash(Achievements achievements)
        {
            var achievementStringCollection = achievements.AchievementsCompleted.Zip(achievements.AchievementsCompletedTimestamp, (id, timestamp) => $"{id}{timestamp.ToDateTimeFromUnixTimestamp()}");
            return string.Join(";", achievementStringCollection)
                         .CalculateMD5Hash();
        }

        private Entities.Character EnrichCharacter((GuildMember, Character, Entities.Character character, GuildMemberState state) data)
        {
            var (guildMember, characterInfo, character, _) = data;

            character.LastUpdate = DateTime.Now;
            character.Level = characterInfo.Level;
            if (!string.IsNullOrWhiteSpace(guildMember.Character.Specialization?.Name))
            {
                var specialization = character.Specializations.FirstOrDefault(s => s.Name == guildMember.Character.Specialization?.Name);

                if (specialization == null)
                    character.Specializations.Add(new Specialization
                                                  {
                                                      Name = guildMember.Character.Specialization?.Name,
                                                      ItemLevel = characterInfo.Items.AverageItemLevelEquipped,
                                                      Role = guildMember.Character.Specialization?.Role
                                                  });
                else
                    specialization.ItemLevel = characterInfo.Items.AverageItemLevelEquipped;
            }

            character.AchievementPoints = characterInfo.AchievementPoints;

            character.AchievementsHash = GenerateAchievementsHash(characterInfo.Achievements);
            character.PetsHash = GeneratePetsHash(characterInfo.Pets);
            character.Rank = guildMember.Rank;

            return character;
        }

        private (GuildMember, Character, Entities.Character character, GuildMemberState state) CreateNewCharacter(IRepository repository, (GuildMember guildMember, Character characterInfo, Entities.Character character, GuildMemberState state) data)
        {
            data.character = repository.Characters.Create();

            data.character.Joined = DateTime.Now;
            data.character.Name = data.guildMember.Character.Name;
            data.character.Class = _wowClientFactory.GetCharacterClasses()
                                                    .First(c => c.Id == (int)data.characterInfo.Class)
                                                    .Name;
            data.character.Specializations = new List<Specialization>();
            data.character.Invitations = new List<Invitation>();
            data.character.DiscordMessages = new List<CharacterDiscordMessage>();

            repository.Characters.Add(data.character);

            return data;
        }

        private List<(GuildMember guildMember, Entities.Character character)> GetGuildCharacters(IRepository repository, IEnumerable<GuildMember> guildMembers)
        {
            return guildMembers.FullJoin(repository.Characters, m => m.Character.Name, c => c.Name, m => (guildMember: m, character: null), c => (guildMember: null, character: c), (m, c) => (guildMember: m, character: c))
                               .ToList();
        }
    }
}
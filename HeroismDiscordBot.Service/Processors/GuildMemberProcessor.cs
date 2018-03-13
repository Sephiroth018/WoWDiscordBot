using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Discord;
using Discord.WebSocket;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;
using MoreLinq;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = WowDotNetAPI.Models.Character;

namespace HeroismDiscordBot.Service.Processors
{
    [SuppressMessage("ReSharper", "UnusedTupleComponentInReturnValue")]
    public class GuildMemberProcessor : IProcessor
    {
        private const string MemberJoinedTitle = "Neuzugang! Willkommen!";
        private const string MemberLeftTitle = "Gildenmitglied hat uns verlassen!";
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _discordClient;
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IExplorer _wowClient;

        public GuildMemberProcessor(IConfiguration configuration,
                                    Func<IRepository> repositoryFactory,
                                    IExplorer wowClient,
                                    DiscordSocketClient discordClient)
        {
            _configuration = configuration;
            _repositoryFactory = repositoryFactory;
            _wowClient = wowClient;
            _discordClient = discordClient;
        }

        public void DoWork()
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var guildMembers = RetryHelper.WithRetry(() => _wowClient.GetGuild(_configuration.WoWRegion, _configuration.WoWRealm, _configuration.WoWGuild, GuildOptions.GetEverything), 3)
                                              .Members
                                              .ToList();
                var guildMembersWithState = GetGuildCharacters(repository, guildMembers)
                                            .AsParallel()
                                            .WithDegreeOfParallelism(5)
                                            .Select(GetWoWCharacterData)
                                            .Select(GetGuildMemberState)
                                            .ToList();

                var characters = guildMembersWithState.Where(data => data.state == GuildMemberState.Left)
                                                      .Select(EnrichCharacter)
                                                      .Select(c =>
                                                              {
                                                                  c.Left = DateTime.Now;
                                                                  return c;
                                                              })
                                                      .ToList();

                var characterClasses = RetryHelper.WithRetry(() => _wowClient.GetCharacterClasses(), 3);

                characters.AddRange(guildMembersWithState.Where(data => data.state == GuildMemberState.Joined)
                                                         .Select(data => CreateNewCharacter(repository, data, characterClasses))
                                                         .Select(EnrichCharacter));

                characters.AddRange(guildMembersWithState.Where(data => data.state == GuildMemberState.Changed)
                                                         .Select(EnrichCharacter));

                repository.SaveChanges();

                characters.Where(c => c.Player == null)
                          .ForEach(c =>
                                   {
                                       // ReSharper disable once AccessToDisposedClosure
                                       var existingCharacter = repository.Characters
                                                                         .ToList()
                                                                         // ReSharper disable once AccessToDisposedClosure
                                                                         .Union(repository.Characters.Local)
                                                                         .FirstOrDefault(ec => ec.AchievementPoints == c.AchievementPoints
                                                                                               && ec.AchievementsHash == c.AchievementsHash
                                                                                               && ec.PetsHash == c.PetsHash);

                                       // ReSharper disable once AccessToDisposedClosure
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

                guildMembersWithState.Where(data => data.state == GuildMemberState.Joined || data.state == GuildMemberState.Left)
                                     .Select(data => data.character)
                                     // ReSharper disable once AccessToDisposedClosure
                                     .Select(data => CreateDiscordMessage(repository, data))
                                     .ForEach(SendDiscordMessage);

                repository.SaveChanges();
            }
        }

        private (GuildMember, Character, Entities.Character character, GuildMemberState state) GetGuildMemberState((GuildMember, Character, Entities.Character) data)
        {
            var (guildMember, characterInfo, character) = data;
            var state = GuildMemberState.Unchanged;
            if (guildMember == null)
            {
                state = GuildMemberState.Left;
            }
            else if (character == null || character.Left.HasValue)
            {
                if (character != null)
                {
                    character.Left = null;
                    character.Joined = DateTime.Now;
                }

                state = GuildMemberState.Joined;
            }
            else if (characterInfo.LastModified.ToDateTimeFromUnixTimestamp() > character.LastUpdate)
            {
                state = GuildMemberState.Changed;
            }

            return (guildMember, characterInfo, character, state);
        }

        private (GuildMember, Character, Entities.Character) GetWoWCharacterData((GuildMember guildMember, Entities.Character character) data)
        {
            var (guildMember, character) = data;
            var characterInfo = RetryHelper.WithRetry(() => _wowClient.GetCharacter(_configuration.WoWRegion, _configuration.WoWRealm, guildMember.Character.Name, CharacterOptions.GetEverything), 3);

            return (guildMember, characterInfo, character);
        }

        private void SendDiscordMessage((CharacterDiscordMessage message, Embed messageData) data)
        {
            var guild = _discordClient.GetGuild(_configuration.DiscordGuildId) as IGuild;
            var channel = guild.GetTextChannelAsync(_configuration.DiscordMemberChangeChannelId)
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

                sentMessage?.ModifyAsync(m => m.Embed = data.messageData).Wait();
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
                embed.AddField("Alt(s)", string.Join(Environment.NewLine, alts.Select(c => c.GetNameAndDescription())));

            if (character.Specializations.Any())
                embed.AddField("Spec(s)", string.Join(Environment.NewLine, character.Specializations.Select(s => s.GetDescription())));

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

        private Entities.Character EnrichCharacter((GuildMember, Character, Entities.Character, GuildMemberState) data)
        {
            var (guildMember, characterInfo, character, _) = data;

            character.LastUpdate = characterInfo.LastModified.ToDateTimeFromUnixTimestamp();
            character.Level = characterInfo.Level;

            var talentSet = characterInfo.Talents.FirstOrDefault(t => t.Selected);
            if (talentSet != null)
            {
                var specialization = character.Specializations.FirstOrDefault(s => s.Name == talentSet.Spec.Name);

                if (specialization == null)
                    character.Specializations.Add(new Specialization
                                                  {
                                                      Name = talentSet.Spec.Name,
                                                      ItemLevel = characterInfo.Items.AverageItemLevelEquipped,
                                                      Role = talentSet.Spec.Role
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

        private (GuildMember, Character, Entities.Character character, GuildMemberState state) CreateNewCharacter(IRepository repository,
                                                                                                                  (GuildMember guildMember, Character characterInfo, Entities.Character character, GuildMemberState state) data,
                                                                                                                  IEnumerable<CharacterClassInfo> characterClasses)
        {
            data.character = repository.Characters.Create();

            data.character.Joined = DateTime.Now;
            data.character.Name = data.guildMember.Character.Name;
            data.character.Class = characterClasses
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
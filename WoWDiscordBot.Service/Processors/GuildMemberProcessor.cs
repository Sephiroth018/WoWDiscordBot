using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MoreLinq;
using WoWClient;
using WoWClient.Entities.Character;
using WoWClient.Entities.Guild;
using WoWDiscordBot.Service.Common;
using WoWDiscordBot.Service.Common.Configuration;
using WoWDiscordBot.Service.Discord.MessageHandlers;
using WoWDiscordBot.Service.Entities.DAL;
using Achievements = WoWClient.Entities.Character.Achievements;
using Character = WoWDiscordBot.Service.Entities.DAL.Character;
using Specialization = WoWDiscordBot.Service.Entities.DAL.Specialization;

namespace WoWDiscordBot.Service.Processors
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class GuildMemberProcessor : IProcessor
    {
        private readonly IWoWClientConfiguration _configuration;
        private readonly IDiscordMessageSender<CharacterDiscordMessage> _discordMessageSender;
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IApiClient _wowClient;

        public GuildMemberProcessor(IWoWClientConfiguration configuration,
                                    Func<IRepository> repositoryFactory,
                                    IApiClient wowClient,
                                    IDiscordMessageSender<CharacterDiscordMessage> discordMessageSender)
        {
            _configuration = configuration;
            _repositoryFactory = repositoryFactory;
            _wowClient = wowClient;
            _discordMessageSender = discordMessageSender;
        }

        public ProcessorTypes ProcessorType => ProcessorTypes.GuildMember;

        public void DoWork()
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var guildMembers = RetryHelper.WithRetry(() => _wowClient.GetGuild(_configuration.Realm, _configuration.Guild).Result, 3)
                                              .Members
                                              .ToList();
                var characters = GetGuildCharacters(repository, guildMembers)
                                 // ReSharper disable once AccessToDisposedClosure
                                 .Select(data => CreateNewCharacter(repository, data))
                                 // ReSharper disable once AccessToDisposedClosure
                                 .Select(data => EnrichGuildData(repository, data))
                                 .AsParallel()
                                 .WithDegreeOfParallelism(5)
                                 .Select(c => (GetWoWCharacterData(c), c))
                                 .Where<(WowDotNetAPI.Models.Character character, Character)>(data => data.character != null)
                                 .ToList()
                                 .Select(EnrichCharacter)
                                 .ToList();

                repository.SaveChanges();

                characters.Where(c => c.Player == null)
                          .ForEach(c =>
                                   {
                                       // ReSharper disable once AccessToDisposedClosure
                                       var existingCharacter = repository.Characters
                                                                         .ToList()
                                                                         .FirstOrDefault(ec => ec.AchievementPoints == c.AchievementPoints
                                                                                               && ec.AchievementsHash == c.AchievementsHash
                                                                                               && ec.PetsHash == c.PetsHash
                                                                                               && ec.Player != null);

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
                                                  .Where(c => c.CurrentMembershipState.State == GuildMemberState.Joined)
                                                  // ReSharper disable once PossibleNullReferenceException
                                                  .GroupBy(c => c.Rank.Rank)
                                                  .OrderBy(c => c.Key)
                                                  .ToList();

                                       player.Characters.ForEach(c => c.IsMain = false);

                                       if (main.Any() && main.First().Count() == 1)
                                           main.First()
                                               .First()
                                               .IsMain = true;
                                   });

                repository.SaveChanges();

                characters.Where(data => data.CurrentMembershipState.DiscordMessage == null)
                          // ReSharper disable once AccessToDisposedClosure
                          .Select(data => CreateAndBuildDiscordMessage(repository, data))
                          .ForEach(_discordMessageSender.SendMessage);

                repository.SaveChanges();
            }
        }

        public TimeSpan GetNextOccurence()
        {
            return new TimeSpan(1, 0, 0);
        }

        private (Member, Character) CreateNewCharacter(IRepository repository, (Member, Character) data)
        {
            var (guildMember, character) = data;

            if (character != null)
                return (guildMember, character);

            character = repository.Characters.Create();

            character.Name = guildMember.Character.Name;
            character.Specializations = new List<Specialization>();
            character.Invitations = new List<Invitation>();
            character.GuildMembershipHistory = new List<GuildMembershipState>();
            character.GuildRankHistory = new List<GuildRank>();

            var guildMembership = repository.GuildMembershipHistory.Create();
            guildMembership.State = GuildMemberState.Joined;
            guildMembership.Timestamp = DateTimeOffset.Now;

            character.CurrentMembershipState = guildMembership;

            repository.Characters.Add(character);

            return (guildMember, character);
        }

        private Character EnrichGuildData(IRepository repository, (Member, Character) data)
        {
            var (guildMember, character) = data;

            if (guildMember == null && character.CurrentMembershipState.State != GuildMemberState.Left)
            {
                var guildMembership = repository.GuildMembershipHistory.Create();
                guildMembership.State = GuildMemberState.Left;
                guildMembership.Timestamp = DateTimeOffset.Now;

                character.CurrentMembershipState = guildMembership;
            }

            if (guildMember != null && character.Rank?.Rank != guildMember.Rank)
            {
                var rank = repository.GuildRankHistory.Create();
                // ReSharper disable once PossibleNullReferenceException
                rank.Timestamp = DateTimeOffset.Now;
                rank.Rank = guildMember.Rank;

                character.Rank = rank;
            }

            return character;
        }

        private WowDotNetAPI.Models.Character GetWoWCharacterData(Character character)
        {
            return RetryHelper.WithRetry(() => _wowClient.GetCharacter(_configuration.Region, _configuration.Realm, character.Name, CharacterOptions.GetEverything), 3);
        }

        private CharacterDiscordMessage CreateAndBuildDiscordMessage(IRepository repository, Character character)
        {
            CharacterDiscordMessage message;

            if (character.CurrentMembershipState.DiscordMessage == null)
            {
                message = repository.CharacterDiscordMessages.Create();
                message.GuildMembershipState = character.CurrentMembershipState;

                repository.CharacterDiscordMessages.Add(message);
            }
            else
            {
                message = character.CurrentMembershipState.DiscordMessage;
            }

            return message;
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
            var achievementStringCollection = achievements.AchievementsCompleted.Zip(achievements.AchievementsCompletedTimestamp, (id, timestamp) => $"{id}{DateTimeOffset.FromUnixTimeMilliseconds(timestamp)}");
            return string.Join(";", achievementStringCollection)
                         .CalculateMD5Hash();
        }

        private async Task<Character> EnrichCharacter((WowDotNetAPI.Models.Character, Character) data)
        {
            var (characterInfo, character) = data;

            var characterClasses = await RetryHelper.WithRetry(() => _wowClient.GetCharacterClasses(), 3);

            character.Class = characterClasses
                              .Classes
                              .First(c => c.Id == characterInfo.Class)
                              .Name;
            character.LastWoWUpdate = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(characterInfo.LastModified));
            character.LastUpdate = DateTimeOffset.Now;
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

            return character;
        }

        private List<(Member guildMember, Character character)> GetGuildCharacters(IRepository repository, IEnumerable<Member> guildMembers)
        {
            var x = guildMembers.FullJoin(repository.Characters, m => m.Character.Name, c => c.Name, m => (guildMember: m, character: null), c => (guildMember: null, character: c), (m, c) => (guildMember: m, character: c))
                               .ToList();
            return x;
        }
    }
}
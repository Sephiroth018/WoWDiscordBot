using System;
using System.Collections.Generic;
using System.Linq;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Discord.MessageHandlers;
using HeroismDiscordBot.Service.Entities.DAL;
using MoreLinq;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = HeroismDiscordBot.Service.Entities.DAL.Character;

namespace HeroismDiscordBot.Service.Processors
{
    // ReSharper disable once UnusedMember.Global
    public class GuildMemberProcessor : IProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly IDiscordMessageSender<CharacterDiscordMessage> _discordMessageSender;
        private readonly Func<IRepository> _repositoryFactory;
        private readonly IExplorer _wowClient;

        public GuildMemberProcessor(IConfiguration configuration,
                                    Func<IRepository> repositoryFactory,
                                    IExplorer wowClient,
                                    IDiscordMessageSender<CharacterDiscordMessage> discordMessageSender)
        {
            _configuration = configuration;
            _repositoryFactory = repositoryFactory;
            _wowClient = wowClient;
            _discordMessageSender = discordMessageSender;
        }

        public void DoWork()
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var guildMembers = RetryHelper.WithRetry(() => _wowClient.GetGuild(_configuration.WoWRegion, _configuration.WoWRealm, _configuration.WoWGuild, GuildOptions.GetEverything), 3)
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

                                       if (main.First().Count() == 1)
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

        private (GuildMember, Character) CreateNewCharacter(IRepository repository, (GuildMember, Character) data)
        {
            var (guildMember, character) = data;

            if (character != null)
                return (guildMember, character);

            character = repository.Characters.Create();

            character.Joined = DateTimeOffset.Now;
            character.Name = guildMember.Character.Name;
            character.Specializations = new List<Specialization>();
            character.Invitations = new List<Invitation>();
            character.GuildMembershipHistory = new List<GuildMembershipState>();
            character.GuildRankHistory = new List<GuildRank>();

            character.CurrentMembershipState = repository.GuildMembershipHistory.Create();
            character.CurrentMembershipState.State = GuildMemberState.Joined;
            character.CurrentMembershipState.Timestamp = DateTimeOffset.Now;

            repository.Characters.Add(character);

            return (guildMember, character);
        }

        private Character EnrichGuildData(IRepository repository, (GuildMember, Character) data)
        {
            var (guildMember, character) = data;

            if (guildMember == null && character.CurrentMembershipState.State != GuildMemberState.Left)
            {
                character.CurrentMembershipState = repository.GuildMembershipHistory.Create();
                character.CurrentMembershipState.State = GuildMemberState.Left;
                character.CurrentMembershipState.Timestamp = DateTimeOffset.Now;
            }

            if (guildMember != null && character.Rank?.Rank != guildMember.Rank)
            {
                character.Rank = repository.GuildRankHistory.Create();
                // ReSharper disable once PossibleNullReferenceException
                character.Rank.Timestamp = DateTimeOffset.Now;
                character.Rank.Rank = guildMember.Rank;
            }

            return character;
        }

        private WowDotNetAPI.Models.Character GetWoWCharacterData(Character character)
        {
            return RetryHelper.WithRetry(() => _wowClient.GetCharacter(_configuration.WoWRegion, _configuration.WoWRealm, character.Name, CharacterOptions.GetEverything), 3);
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

        private Character EnrichCharacter((WowDotNetAPI.Models.Character, Character) data)
        {
            var (characterInfo, character) = data;

            var characterClasses = RetryHelper.WithRetry(() => _wowClient.GetCharacterClasses(), 3);

            character.Class = characterClasses
                              .First(c => c.Id == (int)characterInfo.Class)
                              .Name;
            character.LastUpdate = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(characterInfo.LastModified));
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

        private List<(GuildMember guildMember, Character character)> GetGuildCharacters(IRepository repository, IEnumerable<GuildMember> guildMembers)
        {
            return guildMembers.FullJoin(repository.Characters, m => m.Character.Name, c => c.Name, m => (guildMember: m, character: null), c => (guildMember: null, character: c), (m, c) => (guildMember: m, character: c))
                               .ToList();
        }
    }
}
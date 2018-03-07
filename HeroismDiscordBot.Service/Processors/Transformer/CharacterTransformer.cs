using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;
using MoreLinq;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = HeroismDiscordBot.Service.Entities.Character;

namespace HeroismDiscordBot.Service.Processors.Transformer
{
    public class CharacterTransformer
    {
        private readonly WowExplorer _wowClient;
        private readonly IEnumerable<CharacterClassInfo> _classInfo;
        private readonly BotContext _botContext;
        private readonly Configuration _configuration;

        public CharacterTransformer(WowExplorer wowClient, IEnumerable<CharacterClassInfo> classInfo, BotContext botContext, Configuration configuration)
        {
            _wowClient = wowClient;
            _classInfo = classInfo;
            _botContext = botContext;
            _configuration = configuration;
        }

        public async Task<(CharacterData characterData, Character character)> TransformToCharacterData((GuildMember, Character) data)
        {
            var (guildMember, character) = data;
            var characterInfo = await Task.Run(() => _wowClient.GetCharacter(_configuration.WoWRegion, _configuration.WoWRealm, guildMember.Character.Name, CharacterOptions.GetEverything));
            var result = new CharacterData
                         {
                             Name = characterInfo.Name,
                             Level = characterInfo.Level,
                             Class = _classInfo.First(c => c.Id == (int) characterInfo.Class)
                                               .Name,
                             SpecializationRole = guildMember.Character.Specialization.Role,
                             Specialization = guildMember.Character.Specialization.Name,
                             SpecializationLevel = characterInfo.Items.AverageItemLevelEquipped,
                             AchievementPoints = characterInfo.AchievementPoints,
                             AchievementsHash = GenerateAchievementsHash(characterInfo.Achievements),
                             PetsHash = GeneratePetsHash(characterInfo.Pets),
                             Rank = guildMember.Rank
                         };

            return (result, character);
        }

        private string GeneratePetsHash(CharacterPets pets)
        {
            var petsStringCollection = pets.Collected
                                           .OrderBy(p => p.BattlePetId)
                                           .ThenBy(p => p.QualityId)
                                           .Select(p => $"{p.Name}{p.BattlePetId}{p.QualityId}")
                                           .ToList();

            return string.Join(";", petsStringCollection)
                         .CalculateMD5Hash();
        }

        private string GenerateAchievementsHash(Achievements achievements)
        {
            var achievementStringCollection = achievements.AchievementsCompleted.Zip(achievements.AchievementsCompletedTimestamp, (id, timestamp) => $"{id}{timestamp.ToDateTimeFromUnixTimestamp()}");
            return string.Join(";", achievementStringCollection)
                         .CalculateMD5Hash();
        }

        public Character TransformToNewCharacter((CharacterData, Character) data)
        {
            var (characterData, _) = data;

            var character = _botContext.Characters.Create();
            character.Joined = DateTime.Now;
            character.Name = characterData.Name;
            character.Level = characterData.Level;
            character.Class = characterData.Class;
            character.LastUpdate = DateTime.Now;
            character.Specializations = new List<Specialization>
                                        {
                                            new Specialization
                                            {
                                                ItemLevel = characterData.SpecializationLevel,
                                                Name = characterData.Specialization,
                                                Role = characterData.SpecializationRole
                                            }
                                        };
            character.AchievementPoints = characterData.AchievementPoints;
            character.AchievementsHash = characterData.AchievementsHash;
            character.PetsHash = characterData.PetsHash;
            character.Rank = characterData.Rank;

            var existingCharacter = _botContext.Characters
                                               .ToList()
                                               .Union(_botContext.Characters.Local)
                                               .FirstOrDefault(ec => ec.AchievementPoints == character.AchievementPoints && ec.AchievementsHash == character.AchievementsHash && ec.PetsHash == character.PetsHash);
            character.Player = existingCharacter?.Player ?? new Player();
            character.Player.Characters.Add(character);

            var main = character.Player
                                .Characters
                                .GroupBy(c => c.Rank)
                                .OrderBy(c => c.Key)
                                .ToList();

            character.Player.Characters.ForEach(c => c.IsMain = false);
            if (main.First().Count() == 1)
            {
                main.First()
                    .First()
                    .IsMain = true;
            }

            _botContext.Characters.Add(character);

            return character;
        }
    }
}
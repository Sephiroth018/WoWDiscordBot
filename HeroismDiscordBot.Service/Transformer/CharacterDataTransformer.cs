using System.Linq;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Processors;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = HeroismDiscordBot.Service.Entities.Character;

namespace HeroismDiscordBot.Service.Transformer
{
    public class CharacterDataTransformer : ITransformer<(GuildMember, Character), (CharacterData, Character)>
    {
        private readonly IConfiguration _configuration;
        private readonly IWoWFactory _wowClient;

        public CharacterDataTransformer(IWoWFactory wowClient, IConfiguration configuration)
        {
            _wowClient = wowClient;
            _configuration = configuration;
        }

        public (CharacterData, Character) Transform((GuildMember, Character) data)
        {
            var (guildMember, character) = data;
            var characterInfo = _wowClient.GetClient().GetCharacter(_configuration.WoWRegion, _configuration.WoWRealm, guildMember.Character.Name, CharacterOptions.GetEverything);
            var result = new CharacterData
                         {
                             Name = characterInfo.Name,
                             Level = characterInfo.Level,
                             Class = _wowClient.GetCharacterClasses()
                                               .First(c => c.Id == (int)characterInfo.Class)
                                               .Name,
                             SpecializationRole = guildMember.Character.Specialization?.Role,
                             Specialization = guildMember.Character.Specialization?.Name,
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
    }
}
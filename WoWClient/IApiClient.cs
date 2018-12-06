using System.Threading.Tasks;
using WoWClient.Entities;
using Character = WoWClient.Entities.Character.Character;
using Guild = WoWClient.Entities.Guild.Guild;

namespace WoWClient
{
    public interface IApiClient
    {
        Task<MythicChallengeMode> GetMythicChallengeModeIndex(Locale? locale = null);

        Task<ZoneData> GetZones(Locale? locale = null);

        Task<Character> GetCharacter(string realm, string name, Locale? locale = null);

        Task<Guild> GetGuild(string realm, string name, Locale? locale = null);

        Task<CharacterClassData> GetCharacterClasses(Locale? locale = null);
    }
}
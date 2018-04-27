using Newtonsoft.Json;
using WoWClient.Entities.Character;

namespace WoWClient.Entities.Guild
{
    public class Character : CharacterBase
    {
        [JsonProperty("spec")]
        public Specialization Specialization { get; set; }

        [JsonProperty("guildRealm")]
        public string GuildRealm { get; set; }

        [JsonProperty("guild")]
        public string Guild { get; set; }
    }
}
using Newtonsoft.Json;
using WoWClient.Entities.Character;

namespace WoWClient.Entities.Guild
{
    public class GroupMember
    {
        [JsonProperty("character")]
        public Entities.Guild.Character Character { get; set; }

        [JsonProperty("spec")]
        public Specialization Specialization { get; set; }
    }
}
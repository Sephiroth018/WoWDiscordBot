using Newtonsoft.Json;

namespace WoWClient.Entities.Guild
{
    public class Member
    {
        [JsonProperty("character")]
        public Character Character { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }
    }
}
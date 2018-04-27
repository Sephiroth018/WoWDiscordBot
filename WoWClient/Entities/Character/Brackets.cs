using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Brackets
    {
        [JsonProperty("ARENA_BRACKET_2v2")]
        public ArenaBracket2V2 ArenaBracket2V2 { get; set; }

        [JsonProperty("ARENA_BRACKET_3v3")]
        public ArenaBracket2V2 ArenaBracket3V3 { get; set; }

        [JsonProperty("ARENA_BRACKET_RBG")]
        public ArenaBracket2V2 ArenaBracketRbg { get; set; }

        [JsonProperty("ARENA_BRACKET_2v2_SKIRMISH")]
        public ArenaBracket2V2 ArenaBracket2V2Skirmish { get; set; }

        [JsonProperty("UNKNOWN")]
        public ArenaBracket2V2 Unknown { get; set; }
    }
}
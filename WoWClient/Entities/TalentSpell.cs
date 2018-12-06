using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class TalentSpell
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("subtext")]
        public string SubText { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("castTime")]
        public string CastTime { get; set; }

        [JsonProperty("powerCost")]
        public string PowerCost { get; set; }

        [JsonProperty("cooldown")]
        public string Cooldown { get; set; }
    }
}
using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class ItemSpell
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("castTime")]
        public string CastTime { get; set; }
    }
}
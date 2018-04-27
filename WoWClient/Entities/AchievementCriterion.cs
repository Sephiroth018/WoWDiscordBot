using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class AchievementCriterion
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("orderIndex")]
        public long OrderIndex { get; set; }

        [JsonProperty("max")]
        public long Max { get; set; }
    }
}
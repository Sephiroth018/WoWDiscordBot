using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Achievement
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("reward")]
        public string Reward { get; set; }

        [JsonProperty("rewardItems")]
        public List<object> RewardItems { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("criteria")]
        public List<AchievementCriterion> Criteria { get; set; }

        [JsonProperty("accountWide")]
        public bool AccountWide { get; set; }

        [JsonProperty("factionId")]
        public long FactionId { get; set; }
    }
}
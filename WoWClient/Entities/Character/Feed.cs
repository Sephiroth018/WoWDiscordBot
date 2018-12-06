using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WoWClient.Entities.Enums;

namespace WoWClient.Entities.Character
{
    public class Feed
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FeedType Type { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("achievement")]
        public Achievement Achievement { get; set; }

        [JsonProperty("featOfStrength")]
        public bool? FeatOfStrength { get; set; }

        [JsonProperty("criteria")]
        public AchievementCriterion Criteria { get; set; }

        [JsonProperty("quantity")]
        public long? Quantity { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("itemId")]
        public long? ItemId { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("bonusLists")]
        public List<long> BonusLists { get; set; }
    }
}
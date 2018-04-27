using System;
using Newtonsoft.Json;
using WoWClient.Converters;

namespace WoWClient.Entities
{
    public class Map
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("hasChallengeMode")]
        public bool HasChallengeMode { get; set; }

        [JsonProperty("bronzeCriteria")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan BronzeCriteria { get; set; }

        [JsonProperty("silverCriteria")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan SilverCriteria { get; set; }

        [JsonProperty("goldCriteria")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan GoldCriteria { get; set; }
    }
}
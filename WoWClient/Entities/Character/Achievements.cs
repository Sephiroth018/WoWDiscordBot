using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Achievements
    {
        [JsonProperty("achievementsCompleted")]
        public List<long> AchievementsCompleted { get; set; }

        [JsonProperty("achievementsCompletedTimestamp")]
        public List<long> AchievementsCompletedTimestamp { get; set; }

        [JsonProperty("criteria")]
        public List<long> Criteria { get; set; }

        [JsonProperty("criteriaQuantity")]
        public List<long> CriteriaQuantity { get; set; }

        [JsonProperty("criteriaTimestamp")]
        public List<long> CriteriaTimestamp { get; set; }

        [JsonProperty("criteriaCreated")]
        public List<long> CriteriaCreated { get; set; }
    }
}
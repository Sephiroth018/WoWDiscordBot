using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class ArenaBracket2V2
    {
        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("weeklyPlayed")]
        public long WeeklyPlayed { get; set; }

        [JsonProperty("weeklyWon")]
        public long WeeklyWon { get; set; }

        [JsonProperty("weeklyLost")]
        public long WeeklyLost { get; set; }

        [JsonProperty("seasonPlayed")]
        public long SeasonPlayed { get; set; }

        [JsonProperty("seasonWon")]
        public long SeasonWon { get; set; }

        [JsonProperty("seasonLost")]
        public long SeasonLost { get; set; }
    }
}
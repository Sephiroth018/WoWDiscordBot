using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Statistic
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("lastUpdated")]
        public long LastUpdated { get; set; }

        [JsonProperty("money")]
        public bool Money { get; set; }

        [JsonProperty("highest")]
        public string Highest { get; set; }
    }
}
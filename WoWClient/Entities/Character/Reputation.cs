using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Reputation
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("standing")]
        public long Standing { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("max")]
        public long Max { get; set; }
    }
}
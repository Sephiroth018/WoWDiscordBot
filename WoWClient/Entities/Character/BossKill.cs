using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class BossKill
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("normalKills")]
        public long? NormalKills { get; set; }

        [JsonProperty("normalTimestamp")]
        public long? NormalTimestamp { get; set; }

        [JsonProperty("heroicKills")]
        public long? HeroicKills { get; set; }

        [JsonProperty("heroicTimestamp")]
        public long? HeroicTimestamp { get; set; }

        [JsonProperty("lfrKills")]
        public long? LfrKills { get; set; }

        [JsonProperty("lfrTimestamp")]
        public long? LfrTimestamp { get; set; }

        [JsonProperty("mythicKills")]
        public long? MythicKills { get; set; }

        [JsonProperty("mythicTimestamp")]
        public long? MythicTimestamp { get; set; }
    }
}
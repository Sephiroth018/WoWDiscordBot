using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Boss
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("urlSlug")]
        public string UrlSlug { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("zoneId")]
        public long ZoneId { get; set; }

        [JsonProperty("availableInNormalMode")]
        public bool AvailableInNormalMode { get; set; }

        [JsonProperty("availableInHeroicMode")]
        public bool AvailableInHeroicMode { get; set; }

        [JsonProperty("health")]
        public long Health { get; set; }

        [JsonProperty("heroicHealth")]
        public long HeroicHealth { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("heroicLevel")]
        public long HeroicLevel { get; set; }

        [JsonProperty("journalId", NullValueHandling = NullValueHandling.Ignore)]
        public long? JournalId { get; set; }

        [JsonProperty("npcs")]
        public List<Npc> Npcs { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public Location Location { get; set; }
    }
}
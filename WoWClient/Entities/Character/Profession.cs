using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Profession
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }

        [JsonProperty("max")]
        public long Max { get; set; }

        [JsonProperty("recipes")]
        public List<long> Recipes { get; set; }
    }
}
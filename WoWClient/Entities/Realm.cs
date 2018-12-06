using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Realm
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("battlegroup")]
        public string Battlegroup { get; set; }

        [JsonProperty("locale")]
        public Locale Locale { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("connected_realms")]
        public List<string> ConnectedRealms { get; set; }
    }
}
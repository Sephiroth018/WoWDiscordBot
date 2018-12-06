using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Guild
{
    public class Challenge
    {
        [JsonProperty("realm")]
        public Realm Realm { get; set; }

        [JsonProperty("map")]
        public Map Map { get; set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }
    }
}
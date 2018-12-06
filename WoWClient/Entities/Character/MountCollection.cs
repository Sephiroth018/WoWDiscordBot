using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class MountCollection
    {
        [JsonProperty("numCollected")]
        public long NumCollected { get; set; }

        [JsonProperty("numNotCollected")]
        public long NumNotCollected { get; set; }

        [JsonProperty("collected")]
        public List<Mount> Collected { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class PetCollection
    {
        [JsonProperty("numCollected")]
        public long NumCollected { get; set; }

        [JsonProperty("numNotCollected")]
        public long NumNotCollected { get; set; }

        [JsonProperty("collected")]
        public List<Pet> Collected { get; set; }
    }
}
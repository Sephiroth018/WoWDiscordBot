using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class PetStats
    {
        [JsonProperty("speciesId")]
        public long SpeciesId { get; set; }

        [JsonProperty("breedId")]
        public long BreedId { get; set; }

        [JsonProperty("petQualityId")]
        public long PetQualityId { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("health")]
        public long Health { get; set; }

        [JsonProperty("power")]
        public long Power { get; set; }

        [JsonProperty("speed")]
        public long Speed { get; set; }
    }
}
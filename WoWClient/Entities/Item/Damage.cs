using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class Damage
    {
        [JsonProperty("min")]
        public long Min { get; set; }

        [JsonProperty("max")]
        public long Max { get; set; }

        [JsonProperty("exactMin")]
        public long ExactMin { get; set; }

        [JsonProperty("exactMax")]
        public long ExactMax { get; set; }
    }
}
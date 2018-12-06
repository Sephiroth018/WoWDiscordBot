using Newtonsoft.Json;
using WoWClient.Converters;

namespace WoWClient.Entities.Item
{
    public class WeaponInfo
    {
        [JsonProperty("damage")]
        public Damage Damage { get; set; }

        [JsonProperty("weaponSpeed")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? WeaponSpeed { get; set; }

        [JsonProperty("dps")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Dps { get; set; }
    }
}
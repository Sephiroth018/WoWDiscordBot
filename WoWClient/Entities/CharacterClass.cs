using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WoWClient.Entities.Enums;

namespace WoWClient.Entities
{
    public class CharacterClass
    {
        [JsonProperty("id")]
        public Classes Id { get; set; }

        [JsonProperty("mask")]
        public long Mask { get; set; }

        [JsonProperty("powerType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PowerTypes PowerType { get; set; }

        [JsonProperty("name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Classes Name { get; set; }
    }
}
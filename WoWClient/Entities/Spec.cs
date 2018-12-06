using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WoWClient.Entities.Enums;

namespace WoWClient.Entities
{
    public class Spec
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("role")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }

        [JsonProperty("backgroundImage")]
        public string BackgroundImage { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }
    }
}
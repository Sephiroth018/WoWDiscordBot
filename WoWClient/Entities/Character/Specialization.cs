using Newtonsoft.Json;
using WoWClient.Entities.Enums;

namespace WoWClient.Entities.Character
{
    public class Specialization
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("role")]
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
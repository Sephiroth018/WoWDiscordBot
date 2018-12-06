using Newtonsoft.Json;

namespace WoWClient.Entities.Guild
{
    public class Emblem
    {
        [JsonProperty("icon")]
        public long Icon { get; set; }

        [JsonProperty("iconColor")]
        public string IconColor { get; set; }

        [JsonProperty("iconColorId")]
        public long IconColorId { get; set; }

        [JsonProperty("border")]
        public long Border { get; set; }

        [JsonProperty("borderColor")]
        public string BorderColor { get; set; }

        [JsonProperty("borderColorId")]
        public long BorderColorId { get; set; }

        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("backgroundColorId")]
        public long BackgroundColorId { get; set; }
    }
}
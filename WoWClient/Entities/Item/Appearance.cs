using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class Appearance
    {
        [JsonProperty("itemId")]
        public long? ItemId { get; set; }

        [JsonProperty("itemAppearanceModId")]
        public long? ItemAppearanceModId { get; set; }

        [JsonProperty("transmogItemAppearanceModId")]
        public long? TransmogItemAppearanceModId { get; set; }

        [JsonProperty("enchantDisplayInfoId")]
        public long? EnchantDisplayInfoId { get; set; }
    }
}
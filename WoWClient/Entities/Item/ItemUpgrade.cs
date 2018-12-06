using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class ItemUpgrade
    {
        [JsonProperty("current")]
        public long? Current { get; set; }

        [JsonProperty("total")]
        public long? Total { get; set; }

        [JsonProperty("itemLevelIncrement")]
        public long? ItemLevelIncrement { get; set; }
    }
}
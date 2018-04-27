using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class TooltipParams
    {
        [JsonProperty("set")]
        public List<long> Set { get; set; }

        [JsonProperty("transmogItem")]
        public long? TransmogItem { get; set; }

        [JsonProperty("upgrade")]
        public ItemUpgrade Upgrade { get; set; }

        [JsonProperty("timewalkerLevel")]
        public long TimewalkerLevel { get; set; }

        [JsonProperty("enchant")]
        public long? Enchant { get; set; }

        [JsonProperty("gem0")]
        public long? Gem0 { get; set; }

        [JsonProperty("gem1")]
        public long? Gem1 { get; set; }

        [JsonProperty("gem2")]
        public long? Gem2 { get; set; }
    }
}
using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class ItemStat
    {
        [JsonProperty("stat")]
        public long Stat { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }
}
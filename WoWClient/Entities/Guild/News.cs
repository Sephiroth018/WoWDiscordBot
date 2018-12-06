using System.Collections.Generic;
using Newtonsoft.Json;
using WoWClient.Entities.Enums;

namespace WoWClient.Entities.Guild
{
    public class News
    {
        [JsonProperty("type")]
        public NewsType Type { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("itemId")]
        public long? ItemId { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("bonusLists")]
        public List<long> BonusLists { get; set; }

        [JsonProperty("achievement")]
        public Achievement Achievement { get; set; }
    }
}
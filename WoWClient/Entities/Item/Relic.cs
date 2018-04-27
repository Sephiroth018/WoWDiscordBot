using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class Relic
    {
        [JsonProperty("socket")]
        public long Socket { get; set; }

        [JsonProperty("itemId")]
        public long ItemId { get; set; }

        [JsonProperty("context")]
        public long Context { get; set; }

        [JsonProperty("bonusLists")]
        public List<long> BonusLists { get; set; }
    }
}
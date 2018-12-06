using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Raid
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lfr")]
        public long Lfr { get; set; }

        [JsonProperty("normal")]
        public long Normal { get; set; }

        [JsonProperty("heroic")]
        public long Heroic { get; set; }

        [JsonProperty("mythic")]
        public long Mythic { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("bosses")]
        public List<BossKill> Bosses { get; set; }
    }
}
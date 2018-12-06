using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class ZoneData
    {
        [JsonProperty("zones")]
        public List<Zone> Zones { get; set; }
    }
}
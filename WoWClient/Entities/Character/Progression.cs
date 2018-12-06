using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Progression
    {
        [JsonProperty("raids")]
        public List<Raid> Raids { get; set; }
    }
}
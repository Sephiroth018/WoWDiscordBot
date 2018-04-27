using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Professions
    {
        [JsonProperty("primary")]
        public List<Profession> Primary { get; set; }

        [JsonProperty("secondary")]
        public List<Profession> Secondary { get; set; }
    }
}
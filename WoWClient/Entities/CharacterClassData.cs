using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class CharacterClassData
    {
        [JsonProperty("classes")]
        public List<CharacterClass> Classes { get; set; }
    }
}
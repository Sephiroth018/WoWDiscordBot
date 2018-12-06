using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Pvp
    {
        [JsonProperty("brackets")]
        public Brackets Brackets { get; set; }
    }
}
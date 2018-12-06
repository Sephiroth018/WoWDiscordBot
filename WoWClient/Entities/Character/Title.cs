using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Title
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("selected")]
        public bool? Selected { get; set; }
    }
}
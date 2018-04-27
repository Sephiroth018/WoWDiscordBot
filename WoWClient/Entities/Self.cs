using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Self
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}
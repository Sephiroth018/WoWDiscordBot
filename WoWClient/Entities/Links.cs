using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Links
    {
        [JsonProperty("self")]
        public Self Self { get; set; }
    }
}
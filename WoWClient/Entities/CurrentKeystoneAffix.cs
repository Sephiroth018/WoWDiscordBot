using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class CurrentKeystoneAffix
    {
        [JsonProperty("keystone_affix")]
        public KeystoneAffix KeystoneAffix { get; set; }

        [JsonProperty("starting_level")]
        public long StartingLevel { get; set; }
    }
}
using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class KeystoneAffix
    {
        [JsonProperty("key")]
        public Self Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonIgnore]
        public KeystoneAffixes Affix => (KeystoneAffixes)Id;
    }
}
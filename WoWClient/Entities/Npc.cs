using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Npc
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("urlSlug")]
        public string UrlSlug { get; set; }

        [JsonProperty("creatureDisplayId")]
        public int CreatureDisplayId { get; set; }
    }
}
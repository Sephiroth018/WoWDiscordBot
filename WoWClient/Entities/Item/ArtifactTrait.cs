using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class ArtifactTrait
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }
    }
}
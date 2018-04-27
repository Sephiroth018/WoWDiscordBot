using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class ItemSource
    {
        [JsonProperty("sourceId")]
        public long SourceId { get; set; }

        [JsonProperty("sourceType")]
        public string SourceType { get; set; }
    }
}
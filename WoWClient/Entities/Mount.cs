using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Mount
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("spellId")]
        public long SpellId { get; set; }

        [JsonProperty("creatureId")]
        public long CreatureId { get; set; }

        [JsonProperty("itemId")]
        public long ItemId { get; set; }

        [JsonProperty("qualityId")]
        public long QualityId { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("isGround")]
        public bool IsGround { get; set; }

        [JsonProperty("isFlying")]
        public bool IsFlying { get; set; }

        [JsonProperty("isAquatic")]
        public bool IsAquatic { get; set; }

        [JsonProperty("isJumping")]
        public bool IsJumping { get; set; }
    }
}
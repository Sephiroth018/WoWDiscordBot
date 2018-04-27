using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Pet
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

        [JsonProperty("stats")]
        public PetStats Stats { get; set; }

        [JsonProperty("battlePetGuid")]
        public string BattlePetGuid { get; set; }

        [JsonProperty("isFavorite")]
        public bool IsFavorite { get; set; }

        [JsonProperty("isFirstAbilitySlotSelected")]
        public bool IsFirstAbilitySlotSelected { get; set; }

        [JsonProperty("isSecondAbilitySlotSelected")]
        public bool IsSecondAbilitySlotSelected { get; set; }

        [JsonProperty("isThirdAbilitySlotSelected")]
        public bool IsThirdAbilitySlotSelected { get; set; }

        [JsonProperty("creatureName")]
        public string CreatureName { get; set; }

        [JsonProperty("canBattle")]
        public bool CanBattle { get; set; }
    }
}
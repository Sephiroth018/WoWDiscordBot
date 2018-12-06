using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Audit
    {
        [JsonProperty("numberOfIssues")]
        public long NumberOfIssues { get; set; }

        [JsonProperty("slots")]
        public Slots Slots { get; set; }

        [JsonProperty("emptyGlyphSlots")]
        public long EmptyGlyphSlots { get; set; }

        [JsonProperty("unspentTalentPoints")]
        public long UnspentTalentPoints { get; set; }

        [JsonProperty("noSpec")]
        public bool NoSpec { get; set; }

        [JsonProperty("unenchantedItems")]
        public Slots UnenchantedItems { get; set; }

        [JsonProperty("emptySockets")]
        public long EmptySockets { get; set; }

        [JsonProperty("itemsWithEmptySockets")]
        public Slots ItemsWithEmptySockets { get; set; }

        [JsonProperty("appropriateArmorType")]
        public long AppropriateArmorType { get; set; }

        [JsonProperty("inappropriateArmorType")]
        public Slots InappropriateArmorType { get; set; }

        [JsonProperty("lowLevelItems")]
        public Slots LowLevelItems { get; set; }

        [JsonProperty("lowLevelThreshold")]
        public long LowLevelThreshold { get; set; }

        [JsonProperty("missingExtraSockets")]
        public Slots MissingExtraSockets { get; set; }

        [JsonProperty("recommendedBeltBuckle")]
        public RecommendedItem RecommendedBeltBuckle { get; set; }

        [JsonProperty("missingBlacksmithSockets")]
        public Slots MissingBlacksmithSockets { get; set; }

        [JsonProperty("missingEnchanterEnchants")]
        public Slots MissingEnchanterEnchants { get; set; }

        [JsonProperty("missingEngineerEnchants")]
        public Slots MissingEngineerEnchants { get; set; }

        [JsonProperty("missingScribeEnchants")]
        public Slots MissingScribeEnchants { get; set; }

        [JsonProperty("nMissingJewelcrafterGems")]
        public long NMissingJewelcrafterGems { get; set; }

        [JsonProperty("recommendedJewelcrafterGem")]
        public RecommendedItem RecommendedJewelcrafterGem { get; set; }

        [JsonProperty("missingLeatherworkerEnchants")]
        public Slots MissingLeatherworkerEnchants { get; set; }
    }
}
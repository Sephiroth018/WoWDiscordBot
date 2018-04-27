using System.Collections.Generic;
using Newtonsoft.Json;
using WoWClient.Entities.Item;

namespace WoWClient.Entities.Character
{
    public class RecommendedItem
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("stackable")]
        public long Stackable { get; set; }

        [JsonProperty("itemBind")]
        public long ItemBind { get; set; }

        [JsonProperty("bonusStats")]
        public List<object> BonusStats { get; set; }

        [JsonProperty("itemSpells")]
        public List<ItemSpellStats> ItemSpells { get; set; }

        [JsonProperty("buyPrice")]
        public long BuyPrice { get; set; }

        [JsonProperty("itemClass")]
        public long ItemClass { get; set; }

        [JsonProperty("itemSubClass")]
        public long ItemSubClass { get; set; }

        [JsonProperty("containerSlots")]
        public long ContainerSlots { get; set; }

        [JsonProperty("inventoryType")]
        public long InventoryType { get; set; }

        [JsonProperty("equippable")]
        public bool Equippable { get; set; }

        [JsonProperty("itemLevel")]
        public long ItemLevel { get; set; }

        [JsonProperty("maxCount")]
        public long MaxCount { get; set; }

        [JsonProperty("maxDurability")]
        public long MaxDurability { get; set; }

        [JsonProperty("minFactionId")]
        public long MinFactionId { get; set; }

        [JsonProperty("minReputation")]
        public long MinReputation { get; set; }

        [JsonProperty("quality")]
        public long Quality { get; set; }

        [JsonProperty("sellPrice")]
        public long SellPrice { get; set; }

        [JsonProperty("requiredSkill")]
        public long RequiredSkill { get; set; }

        [JsonProperty("requiredLevel")]
        public long RequiredLevel { get; set; }

        [JsonProperty("requiredSkillRank")]
        public long RequiredSkillRank { get; set; }

        [JsonProperty("itemSource")]
        public ItemSource ItemSource { get; set; }

        [JsonProperty("baseArmor")]
        public long BaseArmor { get; set; }

        [JsonProperty("hasSockets")]
        public bool HasSockets { get; set; }

        [JsonProperty("isAuctionable")]
        public bool IsAuctionable { get; set; }

        [JsonProperty("armor")]
        public long Armor { get; set; }

        [JsonProperty("displayInfoId")]
        public long DisplayInfoId { get; set; }

        [JsonProperty("nameDescription")]
        public string NameDescription { get; set; }

        [JsonProperty("nameDescriptionColor")]
        public string NameDescriptionColor { get; set; }

        [JsonProperty("upgradable")]
        public bool Upgradable { get; set; }

        [JsonProperty("heroicTooltip")]
        public bool HeroicTooltip { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("bonusLists")]
        public List<object> BonusLists { get; set; }

        [JsonProperty("availableContexts")]
        public List<string> AvailableContexts { get; set; }

        [JsonProperty("bonusSummary")]
        public BonusSummary BonusSummary { get; set; }

        [JsonProperty("artifactId")]
        public long ArtifactId { get; set; }
    }
}
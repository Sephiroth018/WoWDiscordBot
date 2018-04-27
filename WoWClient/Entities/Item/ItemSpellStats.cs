using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class ItemSpellStats
    {
        [JsonProperty("spellId")]
        public long SpellId { get; set; }

        [JsonProperty("spell")]
        public ItemSpell Spell { get; set; }

        [JsonProperty("nCharges")]
        public long NCharges { get; set; }

        [JsonProperty("consumable")]
        public bool Consumable { get; set; }

        [JsonProperty("categoryId")]
        public long CategoryId { get; set; }

        [JsonProperty("trigger")]
        public string Trigger { get; set; }
    }
}
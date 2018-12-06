using Newtonsoft.Json;
using WoWClient.Entities.Item;

namespace WoWClient.Entities.Character
{
    public class Items
    {
        [JsonProperty("averageItemLevel")]
        public long AverageItemLevel { get; set; }

        [JsonProperty("averageItemLevelEquipped")]
        public long AverageItemLevelEquipped { get; set; }

        [JsonProperty("head")]
        public Item.Item Head { get; set; }

        [JsonProperty("neck")]
        public Item.Item Neck { get; set; }

        [JsonProperty("shoulder")]
        public Item.Item Shoulder { get; set; }

        [JsonProperty("back")]
        public Item.Item Back { get; set; }

        [JsonProperty("shirt")]
        public Item.Item Shirt { get; set; }

        [JsonProperty("chest")]
        public Item.Item Chest { get; set; }

        [JsonProperty("tabard")]
        public Item.Item Tabard { get; set; }

        [JsonProperty("wrist")]
        public Item.Item Wrist { get; set; }

        [JsonProperty("hands")]
        public Item.Item Hands { get; set; }

        [JsonProperty("waist")]
        public Item.Item Waist { get; set; }

        [JsonProperty("legs")]
        public Item.Item Legs { get; set; }

        [JsonProperty("feet")]
        public Item.Item Feet { get; set; }

        [JsonProperty("finger1")]
        public Item.Item Finger1 { get; set; }

        [JsonProperty("finger2")]
        public Item.Item Finger2 { get; set; }

        [JsonProperty("trinket1")]
        public Item.Item Trinket1 { get; set; }

        [JsonProperty("trinket2")]
        public Item.Item Trinket2 { get; set; }

        [JsonProperty("mainHand")]
        public Weapon MainHand { get; set; }

        [JsonProperty("offHand")]
        public Weapon OffHand { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Item
{
    public class Item
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("quality")]
        public long Quality { get; set; }

        [JsonProperty("itemLevel")]
        public long ItemLevel { get; set; }

        [JsonProperty("armor")]
        public long Armor { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("bonusLists")]
        public List<long> BonusLists { get; set; }

        [JsonProperty("artifactAppearanceId")]
        public long ArtifactAppearanceId { get; set; }

        [JsonProperty("artifactTraits")]
        public List<ArtifactTrait> ArtifactTraits { get; set; }

        [JsonProperty("stats")]
        public List<ItemStat> Stats { get; set; }

        [JsonProperty("displayInfoId")]
        public long DisplayInfoId { get; set; }

        [JsonProperty("artifactId")]
        public long ArtifactId { get; set; }

        [JsonProperty("relics")]
        public List<Relic> Relics { get; set; }

        [JsonProperty("appearance")]
        public Appearance Appearance { get; set; }

        [JsonProperty("tooltipParams")]
        public TooltipParams TooltipParams { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class BonusSummary
    {
        [JsonProperty("defaultBonusLists")]
        public List<object> DefaultBonusLists { get; set; }

        [JsonProperty("chanceBonusLists")]
        public List<object> ChanceBonusLists { get; set; }

        [JsonProperty("bonusChances")]
        public List<object> BonusChances { get; set; }
    }
}
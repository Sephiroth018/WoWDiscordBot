using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class PetSlot
    {
        [JsonProperty("slot")]
        public long Slot { get; set; }

        [JsonProperty("battlePetGuid")]
        public string BattlePetGuid { get; set; }

        [JsonProperty("isEmpty")]
        public bool IsEmpty { get; set; }

        [JsonProperty("isLocked")]
        public bool IsLocked { get; set; }

        [JsonProperty("abilities")]
        public List<long> Abilities { get; set; }
    }
}
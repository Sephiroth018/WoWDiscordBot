using Newtonsoft.Json;

namespace WoWClient.Entities
{
    public class Talent
    {
        [JsonProperty("tier")]
        public long Tier { get; set; }

        [JsonProperty("column")]
        public long Column { get; set; }

        [JsonProperty("spell")]
        public TalentSpell Spell { get; set; }

        [JsonProperty("spec")]
        public Spec Spec { get; set; }
    }
}
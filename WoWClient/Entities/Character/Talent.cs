using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Talent
    {
        [JsonProperty("selected")]
        public bool? Selected { get; set; }

        [JsonProperty("talents")]
        public List<Entities.Talent> Talents { get; set; }

        [JsonProperty("spec")]
        public Spec Spec { get; set; }

        [JsonProperty("calcTalent")]
        public string CalcTalent { get; set; }

        [JsonProperty("calcSpec")]
        public string CalcSpec { get; set; }
    }
}
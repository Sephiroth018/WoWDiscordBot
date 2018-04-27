using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Appearance
    {
        [JsonProperty("faceVariation")]
        public long FaceVariation { get; set; }

        [JsonProperty("skinColor")]
        public long SkinColor { get; set; }

        [JsonProperty("hairVariation")]
        public long HairVariation { get; set; }

        [JsonProperty("hairColor")]
        public long HairColor { get; set; }

        [JsonProperty("featureVariation")]
        public long FeatureVariation { get; set; }

        [JsonProperty("showHelm")]
        public bool ShowHelm { get; set; }

        [JsonProperty("showCloak")]
        public bool ShowCloak { get; set; }

        [JsonProperty("customDisplayOptions")]
        public List<long> CustomDisplayOptions { get; set; }
    }
}
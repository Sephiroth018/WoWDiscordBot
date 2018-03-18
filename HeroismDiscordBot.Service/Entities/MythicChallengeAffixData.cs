using System;
using System.Collections.Generic;
using HeroismDiscordBot.Service.Common;
using Newtonsoft.Json;

namespace HeroismDiscordBot.Service.Entities
{
    public class MythicChallengeAffixData
    {
        [JsonProperty("current_period")]
        public int CurrentPeriod { get; set; }

        [JsonProperty("current_period_start_timestamp")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTimeOffset From { get; set; }

        [JsonProperty("current_period_end_timestamp")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTimeOffset Until { get; set; }

        [JsonProperty("current_keystone_affixes")]
        public List<Affix> Affixes { get; set; }
    }

    public class Affix
    {
        [JsonProperty("keystone_affix")]
        public KeystoneAffix KeystoneAffix { get; set; }

        [JsonProperty("starting_level")]
        public int StartingLevel { get; set; }
    }

    public class KeystoneAffix
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
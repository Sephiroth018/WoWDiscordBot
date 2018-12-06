using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WoWClient.Converters;

namespace WoWClient.Entities
{
    public class MythicChallengeMode
    {
        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("current_period")]
        public long CurrentPeriod { get; set; }

        [JsonProperty("current_period_start_timestamp")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTimeOffset CurrentPeriodStartTimestamp { get; set; }

        [JsonProperty("current_period_end_timestamp")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTimeOffset CurrentPeriodEndTimestamp { get; set; }

        [JsonProperty("current_keystone_affixes")]
        public List<CurrentKeystoneAffix> CurrentKeystoneAffixes { get; set; }
    }
}
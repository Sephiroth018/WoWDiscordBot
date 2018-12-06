using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WoWClient.Converters;

namespace WoWClient.Entities.Guild
{
    public class Group
    {
        [JsonProperty("ranking")]
        public long Ranking { get; set; }

        [JsonProperty("time")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Time { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("faction")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Faction Faction { get; set; }

        [JsonProperty("isRecurring")]
        public bool IsRecurring { get; set; }

        [JsonProperty("members")]
        public List<GroupMember> Members { get; set; }

        [JsonProperty("guild")]
        public GroupGuild Guild { get; set; }
    }
}
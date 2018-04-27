using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WoWClient.Converters;

namespace WoWClient.Entities.Guild
{
    public class Guild
    {
        [JsonProperty("lastModified")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTimeOffset LastModified { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("battlegroup")]
        public string Battlegroup { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("side")]
        public long Side { get; set; }

        [JsonProperty("achievementPoints")]
        public long AchievementPoints { get; set; }

        [JsonProperty("achievements")]
        public Achievements Achievements { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }

        [JsonProperty("emblem")]
        public Emblem Emblem { get; set; }

        [JsonProperty("news")]
        public List<News> News { get; set; }

        [JsonProperty("challenge")]
        public List<Challenge> Challenge { get; set; }
    }
}
using System;
using Newtonsoft.Json;
using WoWClient.Converters;
using WoWClient.Entities.Enums;

namespace WoWClient.Entities
{
    public class CharacterBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("battlegroup")]
        public string Battlegroup { get; set; }

        [JsonProperty("class")]
        public Classes Class { get; set; }

        [JsonProperty("race")]
        public Races Race { get; set; }

        [JsonProperty("gender")]
        public Genders Gender { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("achievementPoints")]
        public long AchievementPoints { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTimeOffset LastModified { get; set; }
    }
}
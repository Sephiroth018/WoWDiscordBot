using Newtonsoft.Json;
using WoWClient.Entities.Guild;

namespace WoWClient.Entities.Character
{
    public class Guild
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("battlegroup")]
        public string Battlegroup { get; set; }

        [JsonProperty("members")]
        public long Members { get; set; }

        [JsonProperty("achievementPoints")]
        public long AchievementPoints { get; set; }

        [JsonProperty("emblem")]
        public Emblem Emblem { get; set; }
    }

}
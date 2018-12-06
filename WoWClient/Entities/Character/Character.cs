using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWClient.Entities.Character
{
    public class Character : CharacterBase
    {
        [JsonProperty("calcClass")]
        public string CalcClass { get; set; }

        [JsonProperty("faction")]
        public long Faction { get; set; }

        [JsonProperty("feed")]
        public List<Feed> Feed { get; set; }

        [JsonProperty("items")]
        public Items Items { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("professions")]
        public Professions Professions { get; set; }

        [JsonProperty("reputation")]
        public List<Reputation> Reputation { get; set; }

        [JsonProperty("titles")]
        public List<Title> Titles { get; set; }

        [JsonProperty("achievements")]
        public Achievements Achievements { get; set; }

        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }

        [JsonProperty("talents")]
        public List<Talent> Talents { get; set; }

        [JsonProperty("appearance")]
        public Appearance Appearance { get; set; }

        [JsonProperty("mounts")]
        public MountCollection Mounts { get; set; }

        [JsonProperty("pets")]
        public PetCollection Pets { get; set; }

        [JsonProperty("petSlots")]
        public List<PetSlot> PetSlots { get; set; }

        [JsonProperty("progression")]
        public Progression Progression { get; set; }

        [JsonProperty("pvp")]
        public Pvp Pvp { get; set; }

        [JsonProperty("quests")]
        public List<long> Quests { get; set; }

        [JsonProperty("audit")]
        public Audit Audit { get; set; }

        [JsonProperty("totalHonorableKills")]
        public long? TotalHonorableKills { get; set; }

        [JsonProperty("guild")]
        public Guild Guild { get; set; }
    }
}
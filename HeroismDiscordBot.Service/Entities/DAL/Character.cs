using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly]
    public class Character : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();

        public string Class { get; set; }

        public int Level { get; set; }

        public DateTimeOffset LastUpdate { get; set; }

        public virtual Player Player { get; set; }

        public int AchievementPoints { get; set; }

        public string AchievementsHash { get; set; }

        public string PetsHash { get; set; }

        [CanBeNull]
        [NotMapped]
        public GuildRank Rank
        {
            get { return CurrentMembershipState.State == GuildMemberState.Left ? null : GuildRankHistory.OrderByDescending(r => r.Timestamp).FirstOrDefault(); }
            set
            {
                if (value == null || GuildRankHistory.Contains(value))
                    return;

                GuildRankHistory.Add(value);
            }
        }

        [NotMapped]
        public GuildMembershipState CurrentMembershipState
        {
            get { return GuildMembershipHistory.OrderByDescending(m => m.Timestamp).First(); }
            set
            {
                if (value == null || GuildMembershipHistory.Contains(value))
                    return;

                GuildMembershipHistory.Add(value);
            }
        }

        public virtual ICollection<GuildMembershipState> GuildMembershipHistory { get; set; } = new List<GuildMembershipState>();

        public virtual ICollection<GuildRank> GuildRankHistory { get; set; } = new List<GuildRank>();

        public bool IsMain { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        public string GetNameAndDescription()
        {
            var result = $"{(IsMain ? "**Main: **" : CurrentMembershipState.State == GuildMemberState.Left ? "**Nicht in Gilde: **" : string.Empty)}{Name}: {Class}";

            if (Specializations.Any())
                result = $"{result} {string.Join(",", Specializations.Select(s => s.GetDescription()))}";

            return result;
        }
    }
}
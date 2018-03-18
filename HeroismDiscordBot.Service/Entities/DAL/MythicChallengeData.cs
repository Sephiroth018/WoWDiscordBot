using System;
using System.Collections.Generic;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    public class MythicChallengeData : BaseEntity
    {
        public DateTimeOffset From { get; set; }

        public DateTimeOffset Until { get; set; }

        public virtual ICollection<MythicChallengeAffix> Affixes { get; set; } = new List<MythicChallengeAffix>();

        public int Period { get; set; }
    }
}
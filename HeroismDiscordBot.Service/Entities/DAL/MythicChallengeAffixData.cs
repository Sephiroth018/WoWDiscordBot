using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly]
    public class MythicChallengeAffixData : BaseEntity
    {
        public DateTimeOffset From { get; set; }

        public DateTimeOffset Until { get; set; }

        public virtual ICollection<MythicChallengeAffix> Affixes { get; set; } = new List<MythicChallengeAffix>();

        public int Period { get; set; }
    }
}
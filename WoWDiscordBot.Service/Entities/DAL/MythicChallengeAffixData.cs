using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class MythicChallengeAffixData : BaseEntity
    {
        public DateTimeOffset From { get; set; }

        public DateTimeOffset Until { get; set; }

        public virtual ICollection<MythicChallengeAffix> Affixes { get; set; } = new List<MythicChallengeAffix>();

        public long Period { get; set; }
    }
}
using System;
using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class GuildRank : BaseEntity
    {
        public int Rank { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public virtual Character Character { get; set; }
    }
}
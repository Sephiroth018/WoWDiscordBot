using System;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly]
    public class GuildRank : BaseEntity
    {
        public int Rank { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public virtual Character Character { get; set; }
    }
}
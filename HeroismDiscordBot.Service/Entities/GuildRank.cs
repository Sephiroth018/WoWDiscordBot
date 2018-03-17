using System;

namespace HeroismDiscordBot.Service.Entities
{
    public class GuildRank : BaseEntity
    {
        public int Rank { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public virtual Character Character { get; set; }
    }
}
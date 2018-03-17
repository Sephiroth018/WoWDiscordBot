using System;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Entities
{
    public class GuildMembershipState : BaseEntity
    {
        public DateTimeOffset Timestamp { get; set; }

        public GuildMemberState State { get; set; }

        public virtual Character Character { get; set; }

        [CanBeNull]
        public virtual CharacterDiscordMessage DiscordMessage { get; set; }
    }
}
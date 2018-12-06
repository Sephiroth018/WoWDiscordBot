using System;
using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class GuildMembershipState : BaseEntity
    {
        public DateTimeOffset Timestamp { get; set; }

        public GuildMemberState State { get; set; }

        public virtual Character Character { get; set; }

        [CanBeNull]
        public virtual CharacterDiscordMessage DiscordMessage { get; set; }
    }
}
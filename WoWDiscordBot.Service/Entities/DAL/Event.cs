using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class Event : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Start { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual EventDiscordMessage DiscordMessage { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace HeroismDiscordBot.Service.Entities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual EventDiscordMessage DiscordMessage { get; set; }
    }
}
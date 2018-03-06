using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeroismDiscordBot.Service.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
    }
}
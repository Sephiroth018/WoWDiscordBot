using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeroismDiscordBot.Service.Entities
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime Joined { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
        public string Class { get; set; }
        public int Level { get; set; }
        public DateTime? Left { get; set; }
        public DateTime LastUpdate { get; set; }
        public virtual Player Player { get; set; }
        public int AchievementPoints { get; set; }
        public string AchievementsHash { get; set; }
        public string PetsHash { get; set; }
        public int Rank { get; set; }
        public virtual ICollection<CharacterDiscordMessage> DiscordMessages { get; set; } = new List<CharacterDiscordMessage>();
        public int PlayerId { get; set; }
        public bool IsMain { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}
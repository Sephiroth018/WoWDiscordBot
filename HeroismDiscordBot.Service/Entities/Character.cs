using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WowDotNetAPI.Models;

namespace HeroismDiscordBot.Service.Entities
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Joined { get; set; }
        public ICollection<Specialization> Specializations { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public DateTime? Left { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
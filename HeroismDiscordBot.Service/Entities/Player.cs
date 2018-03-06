using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeroismDiscordBot.Service.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}
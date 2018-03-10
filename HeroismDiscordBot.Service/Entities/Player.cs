using System.Collections.Generic;

namespace HeroismDiscordBot.Service.Entities
{
    public class Player : BaseEntity
    {
        public virtual ICollection<Character> Characters { get; set; }
    }
}
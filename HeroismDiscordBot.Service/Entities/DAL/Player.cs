using System.Collections.Generic;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    public class Player : BaseEntity
    {
        public virtual ICollection<Character> Characters { get; set; }
    }
}
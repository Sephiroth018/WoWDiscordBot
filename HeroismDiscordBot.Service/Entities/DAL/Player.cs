using System.Collections.Generic;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly]
    public class Player : BaseEntity
    {
        public virtual ICollection<Character> Characters { get; set; }
    }
}
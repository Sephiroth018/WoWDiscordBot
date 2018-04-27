using System.Collections.Generic;
using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class Player : BaseEntity
    {
        public virtual ICollection<Character> Characters { get; set; }
    }
}
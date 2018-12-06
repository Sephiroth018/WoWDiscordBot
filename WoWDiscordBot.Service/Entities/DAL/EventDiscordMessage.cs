using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class EventDiscordMessage : BaseDiscordMessage
    {
        [Key]
        [ForeignKey(nameof(Event))]
        public int Id { get; set; }

        public virtual Event Event { get; set; }
    }
}
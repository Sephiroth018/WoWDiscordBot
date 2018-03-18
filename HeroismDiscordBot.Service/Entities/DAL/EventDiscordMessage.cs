using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    public class EventDiscordMessage : BaseDiscordMessage
    {
        [Key]
        [ForeignKey(nameof(Event))]
        public int Id { get; set; }

        public virtual Event Event { get; set; }
    }
}
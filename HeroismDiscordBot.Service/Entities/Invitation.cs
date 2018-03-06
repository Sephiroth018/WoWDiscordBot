using System.ComponentModel.DataAnnotations;

namespace HeroismDiscordBot.Service.Entities
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; }
        public virtual Character Character { get; set; }
        public virtual Event Event { get; set; }
        public InvitationStatus Status { get; set; }
        public int CharacterId { get; set; }
        public int EventId { get; set; }
    }
}
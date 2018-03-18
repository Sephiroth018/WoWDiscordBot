namespace HeroismDiscordBot.Service.Entities.DAL
{
    public class Invitation : BaseEntity
    {
        public virtual Character Character { get; set; }

        public virtual Event Event { get; set; }

        public InvitationStatus Status { get; set; }
    }
}
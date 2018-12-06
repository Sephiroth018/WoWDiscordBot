using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class Invitation : BaseEntity
    {
        public virtual Character Character { get; set; }

        public virtual Event Event { get; set; }

        public InvitationStatus Status { get; set; }
    }
}
using JetBrains.Annotations;
using WoWClient.Entities;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class MythicChallengeAffix : BaseEntity
    {
        public long StartingLevel { get; set; }

        public KeystoneAffixes Affix { get; set; }
    }
}
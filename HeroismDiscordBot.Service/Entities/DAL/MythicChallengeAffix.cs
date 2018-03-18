using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly]
    public class MythicChallengeAffix : BaseEntity
    {
        public int StartingLevel { get; set; }

        public MythicChallengeAffixes Affix { get; set; }
    }
}
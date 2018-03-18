namespace HeroismDiscordBot.Service.Entities.DAL
{
    public class MythicChallengeAffix : BaseEntity
    {
        public int StartingLevel { get; set; }

        public MythicChallengeAffixes Affix { get; set; }
    }
}
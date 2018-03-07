namespace HeroismDiscordBot.Service.Entities
{
    public class CharacterDiscordMessage : BaseDiscordMessage
    {
        public virtual Character Character { get; set; }
    }
}
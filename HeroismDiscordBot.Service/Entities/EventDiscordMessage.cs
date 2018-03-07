namespace HeroismDiscordBot.Service.Entities
{
    public class EventDiscordMessage : BaseDiscordMessage
    {
        public virtual Event Event { get; set; }
    }
}
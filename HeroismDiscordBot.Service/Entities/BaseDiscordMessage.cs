namespace HeroismDiscordBot.Service.Entities
{
    public abstract class BaseDiscordMessage
    {
        public long MessageId { get; set; }

        public long ChannelId { get; set; }
    }
}
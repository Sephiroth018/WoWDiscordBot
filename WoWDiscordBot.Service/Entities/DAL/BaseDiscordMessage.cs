namespace WoWDiscordBot.Service.Entities.DAL
{
    public abstract class BaseDiscordMessage
    {
        public long MessageId { get; set; }

        public long ChannelId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace HeroismDiscordBot.Service.Entities
{
    public abstract class BaseDiscordMessage : BaseEntity
    {
        public long MessageId { get; set; }

        public long ChannelId { get; set; }

        public DiscordMessageType DiscordMessageType { get; set; }
    }
}
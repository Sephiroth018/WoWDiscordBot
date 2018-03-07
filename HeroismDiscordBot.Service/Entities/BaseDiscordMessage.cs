using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroismDiscordBot.Service.Entities
{
    public abstract class BaseDiscordMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MessageId { get; set; }
        public long ChannelId { get; set; }
        public DiscordMessageType DiscordMessageType { get; set; }
    }
}
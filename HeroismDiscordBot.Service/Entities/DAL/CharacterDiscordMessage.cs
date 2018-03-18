using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    public class CharacterDiscordMessage : BaseDiscordMessage
    {
        [Key]
        [ForeignKey(nameof(GuildMembershipState))]
        public int Id { get; set; }

        public virtual GuildMembershipState GuildMembershipState { get; set; }
    }
}
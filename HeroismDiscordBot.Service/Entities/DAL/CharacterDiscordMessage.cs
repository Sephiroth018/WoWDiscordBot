using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class CharacterDiscordMessage : BaseDiscordMessage
    {
        [Key]
        [ForeignKey(nameof(GuildMembershipState))]
        public int Id { get; set; }

        public virtual GuildMembershipState GuildMembershipState { get; set; }
    }
}
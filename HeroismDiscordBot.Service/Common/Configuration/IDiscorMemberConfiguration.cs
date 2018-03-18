using System.Drawing;

namespace HeroismDiscordBot.Service.Common.Configuration
{
    public interface IDiscorMemberConfiguration
    {
        ulong NotificationChannelId { get; set; }

        Color MemberLeftColor { get; set; }

        Color MemberJoinedColor { get; set; }

        bool ShowNonGuildAlts { get; set; }

        bool ShowNonGuildCharacters { get; set; }
    }
}
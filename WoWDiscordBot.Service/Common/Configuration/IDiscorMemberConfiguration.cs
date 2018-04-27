using System.Drawing;

namespace WoWDiscordBot.Service.Common.Configuration
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
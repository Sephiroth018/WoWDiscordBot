using System.Drawing;

namespace HeroismDiscordBot.Service.Common.Configuration
{
    public interface IDiscordConfiguration
    {
        ulong GuildId { get; set; }

        string Token { get; set; }

        ulong ErrorMessageTargetId { get; set; }

        string CommandPrefix { get; set; }

        Color BotMessageColor { get; set; }
    }
}
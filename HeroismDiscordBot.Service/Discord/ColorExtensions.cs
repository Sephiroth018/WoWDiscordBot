using Discord;

namespace HeroismDiscordBot.Service.Discord
{
    public static class ColorExtensions
    {
        public static Color ToDiscordColor(this System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B);
        }
    }
}
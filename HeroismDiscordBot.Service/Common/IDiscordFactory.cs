using Discord;
using Discord.WebSocket;

namespace HeroismDiscordBot.Service.Common {
    public interface IDiscordFactory {
        DiscordSocketClient GetClient();

        IGuild GetGuild();
    }
}
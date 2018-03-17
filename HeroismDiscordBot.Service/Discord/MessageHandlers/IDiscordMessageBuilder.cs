using Discord;

namespace HeroismDiscordBot.Service.Discord.MessageHandlers
{
    public interface IDiscordMessageBuilder<T>
    {
        Embed BuildMessage(T data);
    }
}
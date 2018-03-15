using Discord;

namespace HeroismDiscordBot.Service.Discord.MessageBuilders
{
    public interface IDiscordMessageBuilder<T>
    {
        Embed BuildMessage(T data);
    }
}
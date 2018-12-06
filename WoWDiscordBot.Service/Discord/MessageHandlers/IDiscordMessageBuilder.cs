using Discord;

namespace WoWDiscordBot.Service.Discord.MessageHandlers
{
    public interface IDiscordMessageBuilder<T>
    {
        Embed BuildMessage(T data);
    }
}
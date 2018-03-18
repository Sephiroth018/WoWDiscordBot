using System;

namespace HeroismDiscordBot.Service.Discord.MessageHandlers {
    public interface IDiscordMessageSender<T>
    {
        void SendMessage(T data);

        IDisposable EnterTypingState();
    }
}
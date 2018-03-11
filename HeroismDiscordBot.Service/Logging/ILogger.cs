using System;

namespace HeroismDiscordBot.Service.Logging
{
    public interface ILogger
    {
        void LogException(Exception ex);
    }
}
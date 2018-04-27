using System;

namespace WoWDiscordBot.Service.Logging
{
    public interface ILogger
    {
        void LogException(Exception ex);
    }
}
using System;
using NLog;

namespace WoWDiscordBot.Service.Logging
{
    public class NLogProxy<T> : ILogger
    {
        private static readonly NLog.ILogger Logger = LogManager.GetLogger(typeof(T).FullName);

        public void LogException(Exception ex)
        {
            Logger.Error(ex);
        }
    }
}
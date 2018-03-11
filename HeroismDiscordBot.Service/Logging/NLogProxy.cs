using System;
using NLog;

namespace HeroismDiscordBot.Service.Logging
{
    public class NLogProxy<T> : ILogger
    {
        private static readonly NLog.ILogger logger =
            LogManager.GetLogger(typeof(T).FullName);

        public void LogException(Exception ex)
        {
            logger.Error(ex);
        }
    }
}
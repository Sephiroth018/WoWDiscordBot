using System;
using System.Collections.Generic;
using System.Net;

namespace HeroismDiscordBot.Service.Common
{
    public static class RetryHelper
    {
        public static void WithRetry(Action action, int maxRetries)
        {
            var currentTries = 0;
            var exceptions = new List<Exception>();

            while (currentTries == 0 || currentTries < maxRetries)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                    currentTries++;
                }
            }

            throw new AggregateException(exceptions);
        }

        public static T WithRetry<T>(Func<T> action, int maxRetries)
            where T : class
        {
            var currentTries = 0;
            var exceptions = new List<Exception>();

            while (currentTries == 0 || currentTries < maxRetries)
            {
                try
                {
                    return action();
                }
                catch (Exception e)
                {
                    if (e is WebException wex && wex.Status == WebExceptionStatus.ProtocolError && wex.Response is HttpWebResponse res && res.StatusCode == HttpStatusCode.NotFound)
                        return null;

                    exceptions.Add(e);
                    currentTries++;
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}
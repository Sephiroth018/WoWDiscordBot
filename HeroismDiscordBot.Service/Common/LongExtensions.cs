using System;

namespace HeroismDiscordBot.Service.Common
{
    public static class LongExtensions
    {
        public static DateTime ToDateTimeFromUnixTimestamp(this long value)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(value);
        }
    }
}
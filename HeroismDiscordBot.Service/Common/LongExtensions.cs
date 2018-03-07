using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

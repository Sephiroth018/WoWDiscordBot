using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HeroismDiscordBot.Service.Common
{
    public static class StringExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static string CalculateMD5Hash(this string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            return string.Join("", hash.Select(h => h.ToString("X2")));
        }
    }
}
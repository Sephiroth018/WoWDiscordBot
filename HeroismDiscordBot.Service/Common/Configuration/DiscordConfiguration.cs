using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HeroismDiscordBot.Service.Common.Configuration
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class DiscordConfiguration : IDiscordConfiguration
    {
        public ulong GuildId { get; set; }

        public string Token { get; set; }

        public ulong ErrorMessageTargetId { get; set; }

        public string CommandPrefix { get; set; }

        [JsonConverter(typeof(HashColorConverter))]
        public Color BotMessageColor { get; set; }

        public DiscorMemberConfiguration MemberChangeConfiguration { get; set; } = new DiscorMemberConfiguration();
    }
}
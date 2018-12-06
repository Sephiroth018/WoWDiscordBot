using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WoWDiscordBot.Service.Common.Configuration
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class DiscorMemberConfiguration : IDiscorMemberConfiguration
    {
        public ulong NotificationChannelId { get; set; }

        [JsonConverter(typeof(HashColorConverter))]
        public Color MemberLeftColor { get; set; }

        [JsonConverter(typeof(HashColorConverter))]
        public Color MemberJoinedColor { get; set; }

        public bool ShowNonGuildAlts { get; set; }

        public bool ShowNonGuildCharacters { get; set; }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WoWClient;

namespace WoWDiscordBot.Service.Common.Configuration
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class WoWClientConfiguration : IWoWClientConfiguration
    {
        public string ClientId { get; set; }

        public string Realm { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Region Region { get; set; }

        public string Guild { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Locale Locale { get; set; }

        public string ClientSecret { get; set; }

        public string TokenEndpoint { get; set; }
    }
}
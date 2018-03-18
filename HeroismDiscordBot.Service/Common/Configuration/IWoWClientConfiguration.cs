using WowDotNetAPI;

namespace HeroismDiscordBot.Service.Common.Configuration
{
    public interface IWoWClientConfiguration
    {
        string ClientId { get; set; }

        string Realm { get; set; }

        Region Region { get; set; }

        string Guild { get; set; }

        Locale Locale { get; set; }

        string ClientSecret { get; set; }

        string TokenEndpoint { get; set; }
    }
}
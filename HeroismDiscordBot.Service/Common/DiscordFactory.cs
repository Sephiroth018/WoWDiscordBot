using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace HeroismDiscordBot.Service.Common
{
    public class DiscordFactory : IDiscordFactory
    {
        private readonly IConfiguration _configuration;
        private readonly object _lockObject = new object();
        private DiscordSocketClient _discordClient;

        public DiscordFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DiscordSocketClient GetClient()
        {
            lock (_lockObject)
            {
                if (_discordClient != null)
                    return _discordClient;

                _discordClient = new DiscordSocketClient(new DiscordSocketConfig { ConnectionTimeout = 60000 });
                _discordClient.LoginAsync(TokenType.Bot, _configuration.DiscordToken)
                              .Wait();
                _discordClient.StartAsync()
                              .Wait();
                _discordClient.GetConnectionsAsync()
                              .Wait();

                while (_discordClient.ConnectionState != ConnectionState.Connected)
                {
                    Task.Delay(500)
                        .Wait();
                }

                return _discordClient;
            }
        }

        public IGuild GetGuild()
        {
            var client = GetClient();
            return client.GetGuild(_configuration.DiscordGuildId);
        }
    }
}
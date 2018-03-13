using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SimpleInjector;

namespace HeroismDiscordBot.Service.Common
{
    public class DiscordClientInitializer
    {
        private static TaskCompletionSource<bool> _completionSource = new TaskCompletionSource<bool>();

        public static async Task<DiscordSocketClient> Initialize(Container container)
        {
            var discordClient = new DiscordSocketClient(new DiscordSocketConfig { ConnectionTimeout = 60000 });
            await discordClient.LoginAsync(TokenType.Bot, container.GetInstance<IConfiguration>().DiscordToken);
            await discordClient.StartAsync();

            discordClient.Ready += () =>
                                   {
                                       _completionSource.TrySetResult(true);
                                       return Task.CompletedTask;
                                   };

            await _completionSource.Task;

            var commandService = container.GetInstance<CommandService>();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());

            return discordClient;
        }

        public static async Task Disconnect(DiscordSocketClient client)
        {
            _completionSource = new TaskCompletionSource<bool>();

            client.Disconnected += e =>
                                   {
                                       _completionSource.TrySetResult(true);
                                       return Task.CompletedTask;
                                   };

            await client.StopAsync();

            await _completionSource.Task;
        }
    }
}
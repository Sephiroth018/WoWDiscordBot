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
        private static readonly TaskCompletionSource<bool> CompletionSource = new TaskCompletionSource<bool>();

        public static async Task<DiscordSocketClient> Initialize(Container container)
        {
            var discordClient = new DiscordSocketClient(new DiscordSocketConfig { ConnectionTimeout = 60000 });
            await discordClient.LoginAsync(TokenType.Bot, container.GetInstance<IConfiguration>().DiscordToken);
            await discordClient.StartAsync();

            discordClient.Ready += DiscordClientOnReady;

            await CompletionSource.Task;

            var commandService = container.GetInstance<CommandService>();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());

            return discordClient;
        }

        private static Task DiscordClientOnReady()
        {
            CompletionSource.TrySetResult(true);
            return Task.CompletedTask;
        }
    }
}
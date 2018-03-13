using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using HeroismDiscordBot.Service.Common;

namespace HeroismDiscordBot.Service.Discord.Commands
{
    public class CommandHandler
    {
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _discordClient;
        private readonly IServiceProvider _provider;

        public CommandHandler(DiscordSocketClient discordClient, IConfiguration configuration, CommandService commandService, IServiceProvider provider)
        {
            _discordClient = discordClient;
            _configuration = configuration;
            _commandService = commandService;
            _provider = provider;

            _discordClient.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage msg) || msg.Author.Id == _discordClient.CurrentUser.Id)
                return;

            var context = new SocketCommandContext(_discordClient, msg);
            var argPos = 0;
            
            if (msg.HasStringPrefix(_configuration.DiscordCommandPrefix, ref argPos) || msg.HasMentionPrefix(_discordClient.CurrentUser, ref argPos))
            {
                var result = await _commandService.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}
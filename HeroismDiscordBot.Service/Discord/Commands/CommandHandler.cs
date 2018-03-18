using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using HeroismDiscordBot.Service.Common.Configuration;
using HeroismDiscordBot.Service.Logging;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Discord.Commands
{
    [UsedImplicitly]
    public class CommandHandler
    {
        private readonly CommandService _commandService;
        private readonly IDiscordConfiguration _configuration;
        private readonly DiscordSocketClient _discordClient;
        private readonly ILogger _logger;
        private readonly IServiceProvider _provider;

        public CommandHandler(DiscordSocketClient discordClient, IDiscordConfiguration configuration, CommandService commandService, IServiceProvider provider, ILogger logger)
        {
            _discordClient = discordClient;
            _configuration = configuration;
            _commandService = commandService;
            _provider = provider;
            _logger = logger;

            _discordClient.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage socketMessage)
        {
            try
            {
                if (!(socketMessage is SocketUserMessage msg) || msg.Author.Id == _discordClient.CurrentUser.Id)
                    return;

                var context = new SocketCommandContext(_discordClient, msg);
                var argPos = 0;

                if (msg.HasStringPrefix(_configuration.CommandPrefix, ref argPos) || msg.HasMentionPrefix(_discordClient.CurrentUser, ref argPos))
                {
                    var result = await _commandService.ExecuteAsync(context, argPos, _provider);

                    if (!result.IsSuccess)
                        await context.Channel.SendMessageAsync(result.ToString());
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }
    }
}
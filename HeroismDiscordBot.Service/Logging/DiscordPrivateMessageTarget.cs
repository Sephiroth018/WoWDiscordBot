using System;
using Discord.WebSocket;
using HeroismDiscordBot.Service.Common;
using NLog;
using NLog.Targets;

namespace HeroismDiscordBot.Service.Logging
{
    [Target("DiscordPrivateMessage")]
    public class DiscordPrivateMessageTarget : TargetWithLayout
    {
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _discordClient;

        public DiscordPrivateMessageTarget(DiscordSocketClient discordClient, IConfiguration configuration)
        {
            _discordClient = discordClient;
            _configuration = configuration;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var message = Layout.Render(logEvent);

            var privateChannel = _discordClient.GetUser(_configuration.ErrorMessageTargetId).GetOrCreateDMChannelAsync().Result;
            using (var typing = privateChannel.EnterTypingState())
            {
                privateChannel.SendMessageAsync($"```{message.Substring(0, Math.Min(1990, message.Length))}{(message.Length > 1990 ? "..." : string.Empty)}```").Wait();
            }
        }
    }
}
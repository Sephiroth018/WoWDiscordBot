using System;
using System.Linq;
using Discord;
using HeroismDiscordBot.Service.Common;
using NLog;
using NLog.Targets;

namespace HeroismDiscordBot.Service.Logging
{
    [Target("DiscordPrivateMessage")]
    public class DiscordPrivateMessageTarget : TargetWithLayout
    {
        private readonly IDiscordFactory _discordFactory;
        private readonly IConfiguration _configuration;

        public DiscordPrivateMessageTarget(IDiscordFactory discordFactory, IConfiguration configuration)
        {
            _discordFactory = discordFactory;
            _configuration = configuration;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var message = Layout.Render(logEvent);

            var privateChannel = _discordFactory.GetClient().GetUser(_configuration.ErrorMessageTargetId).GetOrCreateDMChannelAsync().Result;
            using (var typing = privateChannel.EnterTypingState())
            {
                privateChannel.SendMessageAsync($"```{message.Substring(0, Math.Min(1990, message.Length))}{(message.Length > 1990 ? "..." : string.Empty)}```").Wait();
            }
        }
    }
}
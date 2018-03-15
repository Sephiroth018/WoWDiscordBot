using System.Threading.Tasks;
using Discord.Commands;
using Flurl;
using Flurl.Http;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Discord.MessageBuilders;
using HeroismDiscordBot.Service.Entities;

namespace HeroismDiscordBot.Service.Discord.Commands
{
    [Name("CurrentMythicChallenge")]
    public class CurrentMythicChallengeCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IConfiguration _configuration;
        private readonly IDiscordMessageBuilder<MythicChallengeData> _messageBuilder;

        public CurrentMythicChallengeCommand(IDiscordMessageBuilder<MythicChallengeData> messageBuilder, IConfiguration configuration)
        {
            _messageBuilder = messageBuilder;
            _configuration = configuration;
        }

        [Command("currentmythic")]
        [Summary("Gibt die aktuellen M+ Affixe aus.")]
        public async Task CurrentMythic()
        {
            using (var unused = Context.Channel.EnterTypingState())
            {
                var tokenData = await _configuration.WoWOAuthTokenEndpoint
                                                    .SetQueryParam("grant_type", "client_credentials")
                                                    .SetQueryParam("client_id", _configuration.WoWOAuthClientId)
                                                    .SetQueryParam("client_secret", _configuration.WoWOAuthClientSecret)
                                                    .GetJsonAsync();

                var currentMythicData = await "https://eu.api.battle.net/data/wow/mythic-challenge-mode/?namespace=dynamic-eu&locale=de_DE"
                                              .WithOAuthBearerToken((string)tokenData.access_token)
                                              .GetJsonAsync<MythicChallengeData>();

                var message = _messageBuilder.BuildMessage(currentMythicData);

                await ReplyAsync("", false, message);
            }
        }
    }
}
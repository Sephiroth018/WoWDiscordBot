using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using F23.StringSimilarity.Interfaces;
using HeroismDiscordBot.Service.Common.Configuration;
using HeroismDiscordBot.Service.Discord.MessageHandlers;
using HeroismDiscordBot.Service.Entities.DAL;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Discord.Commands
{
    [Name("Whois")]
    [UsedImplicitly]
    public class WhoisCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMetricStringDistance _distanceCalculator;
        private readonly IDiscorMemberConfiguration _memberConfiguration;
        private readonly IDiscordMessageBuilder<Character> _messageBuilder;
        private readonly Func<IRepository> _repositoryFactory;

        public WhoisCommand(Func<IRepository> repositoryFactory, IMetricStringDistance distanceCalculator, IDiscorMemberConfiguration memberConfiguration, IDiscordMessageBuilder<Character> messageBuilder)
        {
            _repositoryFactory = repositoryFactory;
            _distanceCalculator = distanceCalculator;
            _memberConfiguration = memberConfiguration;
            _messageBuilder = messageBuilder;
        }

        [Command("whois")]
        [Summary("Gibt alle gespeicherten Informationen zu dem angegebenen WoW-Charakter aus. Tippfehler werden bis zu einem gewissen Grad ignoriert.")]
        [UsedImplicitly]
        public async Task Whois([Remainder] [Summary("Der Name des WoW-Charakters")]
                                string name)
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var characters = repository.Characters
                                           .Where(c => _memberConfiguration.ShowNonGuildCharacters || c.CurrentMembershipState.State != GuildMemberState.Left)
                                           .ToList()
                                           .Where(c => _distanceCalculator.Distance(c.Name.ToLowerInvariant(), name.ToLowerInvariant()) < name.Length / 2.0);

                var messages = characters.Select(c => _messageBuilder.BuildMessage(c));

                await Task.WhenAll(messages.Select(m => ReplyAsync("", false, m)));
            }
        }
    }
}
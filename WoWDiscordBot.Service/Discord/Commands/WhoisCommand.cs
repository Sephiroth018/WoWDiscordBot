﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using F23.StringSimilarity.Interfaces;
using JetBrains.Annotations;
using WoWDiscordBot.Service.Common.Configuration;
using WoWDiscordBot.Service.Discord.MessageHandlers;
using WoWDiscordBot.Service.Entities.DAL;

namespace WoWDiscordBot.Service.Discord.Commands
{
    [Name("Whois")]
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
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
        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public async Task Whois([Remainder] [Summary("Der Name des WoW-Charakters")]
                                string name)
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var characters = repository.Characters
                                           .Where(c => _memberConfiguration.ShowNonGuildCharacters || c.GuildMembershipHistory.OrderByDescending(m => m.Timestamp).FirstOrDefault().State != GuildMemberState.Left)
                                           .ToList()
                                           .Where(c => _distanceCalculator.Distance(c.Name.ToLowerInvariant(), name.ToLowerInvariant()) < name.Length / 2.0);

                var messages = characters.Select(c => _messageBuilder.BuildMessage(c));

                await Task.WhenAll(messages.Select(m => ReplyAsync("", false, m)));
            }
        }
    }
}
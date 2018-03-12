using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using F23.StringSimilarity.Interfaces;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;

namespace HeroismDiscordBot.Service.DiscordCommands
{
    [Name("Whois")]
    public class WhoisCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IConfiguration _configuration;
        private readonly IMetricStringDistance _distanceCalculator;
        private readonly Func<IRepository> _repositoryFactory;

        public WhoisCommand(IConfiguration configuration, Func<IRepository> repositoryFactory, IMetricStringDistance distanceCalculator)
        {
            _configuration = configuration;
            _repositoryFactory = repositoryFactory;
            _distanceCalculator = distanceCalculator;
        }

        [Command("whois")]
        [Summary("Gibt alle gespeicherten Informationen zu dem angegebenen WoW-Charakter aus. Tippfehler werden bis zu einem gewissen Grad ignoriert.")]
        public async Task Whois([Remainder] [Summary("Der Name des WoW-Charakters")]
                                string name)
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                var characters = repository.Characters
                                           .Where(c => !c.Left.HasValue)
                                           .ToList()
                                           .Where(c => _distanceCalculator.Distance(c.Name.ToLowerInvariant(), name.ToLowerInvariant()) < name.Length / 2);

                var messages = characters.Select(c => BuildPlayerChangedMessage(c, $"**Suchtext:** {name}"));

                await Task.WhenAll(messages.Select(m => ReplyAsync("", false, m)));
            }
        }

        private Embed BuildPlayerChangedMessage(Character character, string title)
        {
            IReadOnlyCollection<Character> alts = GetAlts(character);
            var embed = new EmbedBuilder { Title = title };

            embed.WithColor(_configuration.DiscordBotMessageColor);

            embed.AddInlineField("Wer", character.Name);
            embed.AddInlineField("Beigetreten", character.Joined.ToString("g"));
            embed.AddInlineField("Klasse", character.Class);

            if (alts.Any())
                embed.AddField("Alt(s)", string.Join(Environment.NewLine, alts.Select(c => c.GetNameAndDescription())));

            if (character.Specializations.Any())
                embed.AddField("Spec(s)", string.Join(Environment.NewLine, character.Specializations.Select(s => s.GetDescription())));

            return embed.Build();
        }

        private static List<Character> GetAlts(Character c)
        {
            return c.Player.Characters.Where(a => a.Name != c.Name && !a.Left.HasValue)
                    .OrderBy(a => !a.IsMain)
                    .ToList();
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using MoreLinq;
using WoWDiscordBot.Service.Common.Configuration;

namespace WoWDiscordBot.Service.Discord.Commands
{
    [Name("Help")]
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;
        private readonly IDiscordConfiguration _configuration;

        public HelpCommand(CommandService commandService, IDiscordConfiguration configuration)
        {
            _commandService = commandService;
            _configuration = configuration;
        }

        [Command("help")]
        [Summary("Gibt eine Liste aller verfügbaren Befehle aus")]
        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public async Task GetHelp()
        {
            var builder = new EmbedBuilder
                          {
                              Color = _configuration.BotMessageColor.ToDiscordColor(),
                              Description = $"All meine Befehle werden mit entweder mit `{_configuration.CommandPrefix}<Befehl>` ausgeführt oder indem du mich erwähnst (`@Villain <Befehl>`).{Environment.NewLine}"
                                            + "Ich beherrsche folgende Befehle:"
                          };

            _commandService.Modules.SelectMany(m => m.Commands)
                           .Where(c => c.CheckPreconditionsAsync(Context).Result.IsSuccess)
                           .ForEach(c =>
                                    {
                                        builder.AddField(fieldBuilder =>
                                                         {
                                                             var name = c.Name;

                                                             if (c.Parameters.Any())
                                                                 name += $" {string.Join(" ", c.Parameters.Select(p => $"<{p.Name}>"))}";

                                                             fieldBuilder.Name = name;
                                                             if (c.Parameters.Any())
                                                                 fieldBuilder.Value = $"{string.Join(Environment.NewLine, c.Parameters.Select(p => $"Parameter {p.Name}: {p.Summary}"))}{Environment.NewLine}";

                                                             var aliases = c.Aliases.Where(a => a != c.Name)
                                                                            .ToList();
                                                             if (aliases.Any())
                                                                 fieldBuilder.Value += $"Aliases: {string.Join(", ", aliases)}{Environment.NewLine}";

                                                             fieldBuilder.Value += $"Beschreibung: {c.Summary}";
                                                             //TODO provide example
                                                         });
                                    });

            await ReplyAsync("", embed: builder.Build());
        }
    }
}
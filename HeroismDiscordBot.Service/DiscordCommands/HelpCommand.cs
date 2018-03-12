using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using HeroismDiscordBot.Service.Common;
using MoreLinq;

namespace HeroismDiscordBot.Service.DiscordCommands
{
    [Name("Help")]
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;

        public HelpCommand(CommandService commandService, IConfiguration configuration)
        {
            _commandService = commandService;
            _configuration = configuration;
        }

        [Command("help")]
        [Summary("Gibt eine Liste aller verfügbaren Befehle aus")]
        public async Task GetHelp()
        {
            var builder = new EmbedBuilder
                          {
                              Color = _configuration.DiscordBotMessageColor,
                              Description = $"All meine Befehle werden mit entweder mit `{_configuration.DiscordCommandPrefix}<Befehl>` ausgeführt oder indem du mich erwähnst (`@Villain <Befehl>`).{Environment.NewLine}"
                                            + "Ich beherrsche folgende Befehle:"
                          };

            _commandService.Modules.SelectMany(m => m.Commands)
                           .Where(c => c.CheckPreconditionsAsync(Context).Result.IsSuccess)
                           .ForEach(c =>
                                    {
                                        builder.AddField(fieldBuilder =>
                                                         {
                                                             fieldBuilder.Name = string.Join(", ", c.Aliases);
                                                             fieldBuilder.Value = $"Beschreibung: {c.Summary}";
                                                             //TODO provide example

                                                             if (c.Parameters.Any())
                                                                 fieldBuilder.Value = $"Parameter: {string.Join(", ", c.Parameters.Select(p => p.Name))}{Environment.NewLine}" + fieldBuilder.Value;
                                                         });
                                    });

            await ReplyAsync("", embed: builder.Build());
        }
    }
}
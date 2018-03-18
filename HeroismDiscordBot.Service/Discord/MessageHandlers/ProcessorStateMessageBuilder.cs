using System.Collections.Generic;
using Discord;
using HeroismDiscordBot.Service.Common.Configuration;
using HeroismDiscordBot.Service.Entities.DAL;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Discord.MessageHandlers
{
    [UsedImplicitly]
    public class ProcessorStateMessageBuilder : IDiscordMessageBuilder<ProcessorState>
    {
        private readonly IDiscordConfiguration _configuration;

        private static readonly Dictionary<ProcessorTypes, string> ProcessorTypeTranslation = new Dictionary<ProcessorTypes, string>
                                                                                              {
                                                                                                  { ProcessorTypes.GuildMember, "Gildenmitglieder-Verarbeitung" },
                                                                                                  { ProcessorTypes.MythicChallengeAffixes, "M+-Affixes-Verarbeitung" }
                                                                                              };

        private static readonly Dictionary<ProcessorStates, string> ProcessorStateTranslation = new Dictionary<ProcessorStates, string>
                                                                                                {
                                                                                                    { ProcessorStates.Failed, "Fehlgeschlagen" },
                                                                                                    { ProcessorStates.Running, "In Bearbeitung" },
                                                                                                    { ProcessorStates.Waiting, "Warte auf nächsten Ausführungszeitpunkt" }
                                                                                                };

        public ProcessorStateMessageBuilder(IDiscordConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Embed BuildMessage(ProcessorState data)
        {
            var embed = new EmbedBuilder { Title = ProcessorTypeTranslation[data.ProcessorType] };

            embed.WithColor(_configuration.BotMessageColor.ToDiscordColor());
            embed.AddField("Status", ProcessorStateTranslation[data.State]);
            embed.AddField("Letzte erfolgreiche Ausführung", data.LastCompleted?.ToLocalTime().ToString("g") ?? "Nie");
            embed.AddField("Zuletzt gestartet", data.LastStarted.ToLocalTime().ToString("g"));
            embed.AddField("Nächste Ausführung", data.NextExecution.ToLocalTime().ToString("g"));

            return embed;
        }
    }
}
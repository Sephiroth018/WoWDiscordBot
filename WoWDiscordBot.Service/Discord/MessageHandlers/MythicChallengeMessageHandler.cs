using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using JetBrains.Annotations;
using MoreLinq;
using WoWClient.Entities;
using WoWDiscordBot.Service.Common.Configuration;
using WoWDiscordBot.Service.Entities.DAL;


namespace WoWDiscordBot.Service.Discord.MessageHandlers
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class MythicChallengeMessageHandler : IDiscordMessageBuilder<MythicChallengeAffixData>, IDiscordMessageSender<MythicChallengeAffixData>
    {
        private static readonly Dictionary<KeystoneAffixes, (string name, string description)> AffixDescriptions = new Dictionary<KeystoneAffixes, (string name, string description)>
                                                                                                                          {
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Explosive,
                                                                                                                                  ("Explosiv",
                                                                                                                                  "Gegner, die sich im Kampf befinden, beschwören regelmäßig explosive Kugeln, die explodieren, wenn sie nicht zerstört werden."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Quaking,
                                                                                                                                  ("Bebend",
                                                                                                                                  "Alle Spielercharaktere senden regelmäßig eine Schockwelle aus, die Schaden verursacht und Verbündete in der Nähe unterbricht."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Grievous,
                                                                                                                                  ( "Schrecklich",
                                                                                                                                  "Wenn die Gesundheit von Spielercharakteren unter 90% sinkt, erleiden sie ansteigenden regelmäßigen Schaden, bis sie über 90% ihrer Gesundheit geheilt wurden."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Fortified,
                                                                                                                                  ("Verstärkt", "Normale Gegner haben 20% mehr Gesundheit und verursachen bis zu 30% mehr Schaden.")
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Tyrannical,
                                                                                                                                  ("Tyrannisch", "Bossgegner haben 40% mehr Gesundheit und verursachen bis zu 15% mehr Schaden.")
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Bolstering,
                                                                                                                                  ("Anstachelnd",
                                                                                                                                  "Wenn normale Gegner sterben, erfüllt ihr Todesschrei Verbündete in der Nähe mit Macht und erhöht ihre maximale Gesundheit und ihren Schaden um 20%."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Necrotic,
                                                                                                                                  ("Nekrotisch",
                                                                                                                                  "Alle Nahkampfangriffe der Gegner belegen Ziele mit einer stapelbaren 'Verseuchung', die regelmäßigen Schaden verursacht und die erhaltene Heilung verringert."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Raging,
                                                                                                                                  ("Wütend",
                                                                                                                                  "Normale Gegner bekommen bei 30% Gesundheit einen Wutanfall und verursachen 100% mehr Schaden, bis sie besiegt werden.")
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Skittish,
                                                                                                                                  ("Launisch", "Gegner schenken der von Tanks erzeugten Bedrohung deutlich weniger Aufmerksamkeit." )
                                                                                                                              },
                                                                                                                              { KeystoneAffixes.Teeming, ("Wimmelnd", "Im gesamten Dungeon sind zusätzliche normale Gegner vorhanden.") },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Bursting,
                                                                                                                                  ("Platzend",
                                                                                                                                  "Wenn normale Gegner getötet werden, explodieren sie und fügen allen Spielercharakteren im Verlauf von 4 Sek. 10% ihrer maximalen Gesundheit als Schaden zu. Dieser Effekt ist stapelbar."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Volcanic,
                                                                                                                                  ("Vulkanisch",
                                                                                                                                  "Im Kampf lassen Gegner regelmäßig Flammentropfen unter den Füßen entfernter Spielercharaktere hervorbrechen.")
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Sanguine,
                                                                                                                                  ("Blutig",
                                                                                                                                  "Wenn normale Gegner sterben, hinterlassen sie eine Sekretpfütze, die ihre Verbündeten heilt und Spielercharakteren Schaden zufügt."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Overflowing,
                                                                                                                                  ("Überschüssig",
                                                                                                                                  "Überheilung wird in einen Heilungsabsorptionseffekt umgewandelt."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  KeystoneAffixes.Relentless,
                                                                                                                                  ("Unerbittlich",
                                                                                                                                  "Normale Gegner sind vorübergehend immun gegen Kontrollverlusteffekte."
                                                                                                                                  )
                                                                                                                              }
                                                                                                                          };

        private readonly IDiscordConfiguration _configuration;

        private readonly DiscordSocketClient _discordClient;
        private readonly IDiscorMemberConfiguration _memberConfiguration;

        public MythicChallengeMessageHandler(DiscordSocketClient discordClient, IDiscordConfiguration configuration, IDiscorMemberConfiguration memberConfiguration)
        {
            _discordClient = discordClient;
            _configuration = configuration;
            _memberConfiguration = memberConfiguration;
        }

        public Embed BuildMessage(MythicChallengeAffixData data)
        {
            var embed = new EmbedBuilder { Title = "M+ Affixe" };

            embed.WithColor(_configuration.BotMessageColor.ToDiscordColor());
            embed.WithDescription($"Die folgenden Affixe sind von {data.From.ToLocalTime():g} bis {data.Until.ToLocalTime():g} aktiv:");
            data.Affixes
                .OrderBy(a => a.StartingLevel)
                .ForEach(a => { embed.AddField($"{AffixDescriptions[a.Affix].name}", $"{AffixDescriptions[a.Affix].description} *(Ab +{a.StartingLevel})*"); });
            //TODO refactor entity and co to use localized name from api
            return embed;
        }

        public void SendMessage(MythicChallengeAffixData data)
        {
            var messageData = BuildMessage(data);
            var guild = _discordClient.GetGuild(_configuration.GuildId) as IGuild;
            var channel = guild.GetTextChannelAsync(_memberConfiguration.NotificationChannelId)
                               .Result;
            channel.SendMessageAsync("", embed: messageData)
                   .Wait();
        }

        public IDisposable EnterTypingState()
        {
            var guild = _discordClient.GetGuild(_configuration.GuildId) as IGuild;
            var channel = guild.GetTextChannelAsync(_memberConfiguration.NotificationChannelId)
                               .Result;

            return channel.EnterTypingState();
        }
    }
}
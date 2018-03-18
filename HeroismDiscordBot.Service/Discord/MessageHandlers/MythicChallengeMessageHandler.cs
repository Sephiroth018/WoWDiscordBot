using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities.DAL;
using MoreLinq;

namespace HeroismDiscordBot.Service.Discord.MessageHandlers
{
    public class MythicChallengeMessageHandler : IDiscordMessageBuilder<MythicChallengeData>, IDiscordMessageSender<MythicChallengeData>
    {
        private static readonly Dictionary<MythicChallengeAffixes, (string name, string description)> AffixDescriptions = new Dictionary<MythicChallengeAffixes, (string name, string description)>
                                                                                                                          {
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Explosive,
                                                                                                                                  ("Explosiv",
                                                                                                                                  "Gegner, die sich im Kampf befinden, beschwören regelmäßig explosive Kugeln, die explodieren, wenn sie nicht zerstört werden."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Quaking,
                                                                                                                                  ("Bebend",
                                                                                                                                  "Alle Spielercharaktere senden regelmäßig eine Schockwelle aus, die Schaden verursacht und Verbündete in der Nähe unterbricht."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Grievous,
                                                                                                                                  ( "Schrecklich",
                                                                                                                                  "Wenn die Gesundheit von Spielercharakteren unter 90% sinkt, erleiden sie ansteigenden regelmäßigen Schaden, bis sie über 90% ihrer Gesundheit geheilt wurden."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Fortified,
                                                                                                                                  ("Verstärkt", "Normale Gegner haben 20% mehr Gesundheit und verursachen bis zu 30% mehr Schaden.")
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Tyrannical,
                                                                                                                                  ("Tyrannisch", "Bossgegner haben 40% mehr Gesundheit und verursachen bis zu 15% mehr Schaden.")
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Bolstering,
                                                                                                                                  ("Anstachelnd",
                                                                                                                                  "Wenn normale Gegner sterben, erfüllt ihr Todesschrei Verbündete in der Nähe mit Macht und erhöht ihre maximale Gesundheit und ihren Schaden um 20%."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Necrotic,
                                                                                                                                  ("Nekrotisch",
                                                                                                                                  "Alle Nahkampfangriffe der Gegner belegen Ziele mit einer stapelbaren 'Verseuchung', die regelmäßigen Schaden verursacht und die erhaltene Heilung verringert."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Raging,
                                                                                                                                  ("Wütend",
                                                                                                                                  "Normale Gegner bekommen bei 30% Gesundheit einen Wutanfall und verursachen 100% mehr Schaden, bis sie besiegt werden.")
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Skittish,
                                                                                                                                  ("Launisch", "Gegner schenken der von Tanks erzeugten Bedrohung deutlich weniger Aufmerksamkeit." )
                                                                                                                              },
                                                                                                                              { MythicChallengeAffixes.Teeming, ("Wimmelnd", "Im gesamten Dungeon sind zusätzliche normale Gegner vorhanden.") },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Bursting,
                                                                                                                                  ("Platzend",
                                                                                                                                  "Wenn normale Gegner getötet werden, explodieren sie und fügen allen Spielercharakteren im Verlauf von 4 Sek. 10% ihrer maximalen Gesundheit als Schaden zu. Dieser Effekt ist stapelbar."
                                                                                                                                  )
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Volcanic,
                                                                                                                                  ("Vulkanisch",
                                                                                                                                  "Im Kampf lassen Gegner regelmäßig Flammentropfen unter den Füßen entfernter Spielercharaktere hervorbrechen.")
                                                                                                                              },
                                                                                                                              {
                                                                                                                                  MythicChallengeAffixes.Sanguine,
                                                                                                                                  ("Blutig",
                                                                                                                                  "Wenn normale Gegner sterben, hinterlassen sie eine Sekretpfütze, die ihre Verbündeten heilt und Spielercharakteren Schaden zufügt."
                                                                                                                                  )
                                                                                                                              }
                                                                                                                          };

        private readonly IConfiguration _configuration;

        private readonly DiscordSocketClient _discordClient;

        public MythicChallengeMessageHandler(DiscordSocketClient discordClient, IConfiguration configuration)
        {
            _discordClient = discordClient;
            _configuration = configuration;
        }

        public Embed BuildMessage(MythicChallengeData data)
        {
            var embed = new EmbedBuilder { Title = "M+ Affixe" };

            embed.WithColor(_configuration.DiscordBotMessageColor);
            embed.WithDescription($"Die folgenden Affixe sind von {data.From.ToLocalTime():g} bis {data.Until.ToLocalTime():g} aktiv:");
            data.Affixes
                .OrderBy(a => a.StartingLevel)
                .ForEach(a => { embed.AddField($"{AffixDescriptions[a.Affix].name}", $"{AffixDescriptions[a.Affix].description} *(Ab +{a.StartingLevel})*"); });

            return embed;
        }

        public void SendMessage(MythicChallengeData data)
        {
            var messageData = BuildMessage(data);
            var guild = _discordClient.GetGuild(_configuration.DiscordGuildId) as IGuild;
            var channel = guild.GetTextChannelAsync(_configuration.DiscordMemberChangeChannelId)
                               .Result;
            channel.SendMessageAsync("", embed: messageData)
                   .Wait();
        }

        public IDisposable EnterTypingState()
        {
            var guild = _discordClient.GetGuild(_configuration.DiscordGuildId) as IGuild;
            var channel = guild.GetTextChannelAsync(_configuration.DiscordMemberChangeChannelId)
                               .Result;

            return channel.EnterTypingState();
        }
    }
}
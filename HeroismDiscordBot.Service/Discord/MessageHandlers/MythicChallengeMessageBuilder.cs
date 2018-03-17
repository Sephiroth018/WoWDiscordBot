using System.Collections.Generic;
using System.Linq;
using Discord;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;
using MoreLinq;

namespace HeroismDiscordBot.Service.Discord.MessageHandlers
{
    public class MythicChallengeMessageBuilder : IDiscordMessageBuilder<MythicChallengeData>
    {
        private static readonly Dictionary<string, string> AffixDescriptions = new Dictionary<string, string>
                                                                               {
                                                                                   { "Explosiv", "Gegner, die sich im Kampf befinden, beschwören regelmäßig explosive Kugeln, die explodieren, wenn sie nicht zerstört werden." },
                                                                                   { "Bebend", "Alle Spielercharaktere senden regelmäßig eine Schockwelle aus, die Schaden verursacht und Verbündete in der Nähe unterbricht." },
                                                                                   {
                                                                                       "Schrecklich",
                                                                                       "Wenn die Gesundheit von Spielercharakteren unter 90% sinkt, erleiden sie ansteigenden regelmäßigen Schaden, bis sie über 90% ihrer Gesundheit geheilt wurden."
                                                                                   },
                                                                                   { "Verstärkt", "Normale Gegner haben 20% mehr Gesundheit und verursachen bis zu 30% mehr Schaden." },
                                                                                   { "Tyrannisch", "Bossgegner haben 40% mehr Gesundheit und verursachen bis zu 15% mehr Schaden." },
                                                                                   {
                                                                                       "Anstachelnd",
                                                                                       "Wenn normale Gegner sterben, erfüllt ihr Todesschrei Verbündete in der Nähe mit Macht und erhöht ihre maximale Gesundheit und ihren Schaden um 20%."
                                                                                   },
                                                                                   {
                                                                                       "Nekrotisch",
                                                                                       "Alle Nahkampfangriffe der Gegner belegen Ziele mit einer stapelbaren 'Verseuchung', die regelmäßigen Schaden verursacht und die erhaltene Heilung verringert."
                                                                                   },
                                                                                   { "Wütend", "Normale Gegner bekommen bei 30% Gesundheit einen Wutanfall und verursachen 100% mehr Schaden, bis sie besiegt werden." },
                                                                                   { "Launisch", "Gegner schenken der von Tanks erzeugten Bedrohung deutlich weniger Aufmerksamkeit." },
                                                                                   { "Wimmelnd", "Im gesamten Dungeon sind zusätzliche normale Gegner vorhanden." },
                                                                                   {
                                                                                       "Platzend",
                                                                                       "Wenn normale Gegner getötet werden, explodieren sie und fügen allen Spielercharakteren im Verlauf von 4 Sek. 10% ihrer maximalen Gesundheit als Schaden zu. Dieser Effekt ist stapelbar."
                                                                                   },
                                                                                   { "Vulkanisch", "Im Kampf lassen Gegner regelmäßig Flammentropfen unter den Füßen entfernter Spielercharaktere hervorbrechen." },
                                                                                   { "Blutig", "Wenn normale Gegner sterben, hinterlassen sie eine Sekretpfütze, die ihre Verbündeten heilt und Spielercharakteren Schaden zufügt." }
                                                                               };

        private readonly IConfiguration _configuration;

        public MythicChallengeMessageBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Embed BuildMessage(MythicChallengeData data)
        {
            var embed = new EmbedBuilder { Title = "M+ Affixe" };

            embed.WithColor(_configuration.DiscordBotMessageColor);
            embed.WithDescription($"Die folgenden Affixe sind von {data.From:g} bis {data.Until:g} aktiv:");
            data.Affixes
                .OrderBy(a => a.StartingLevel)
                .ForEach(a => { embed.AddField($"{a.KeystoneAffix.Name}", $"{AffixDescriptions[a.KeystoneAffix.Name]} *(Ab +{a.StartingLevel})*"); });

            return embed;
        }
    }
}
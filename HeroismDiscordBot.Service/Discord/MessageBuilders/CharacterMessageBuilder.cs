using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using HeroismDiscordBot.Service.Entities;

namespace HeroismDiscordBot.Service.Discord.MessageBuilders
{
    public class CharacterMessageBuilder : IDiscordMessageBuilder<Character>
    {
        public Embed BuildMessage(Character data)
        {
            throw new NotImplementedException();
        }

        private static List<Character> GetAlts(Character character)
        {
            return character.Player.Characters.Where(a => a.Name != character.Name && !a.Left.HasValue)
                            .OrderBy(a => !a.IsMain)
                            .ToList();
        }

        private static Embed BuildPlayerChangedMessage(Character character, string title, DateTime timestamp, Color color)
        {
            var alts = GetAlts(character);
            var embed = new EmbedBuilder { Title = title };

            embed.WithColor(color);

            //TODO add status field, make parameter timestamp optional and use lastchanged if not set (with according text)
            embed.AddInlineField("Wer", character.Name);
            embed.AddInlineField("Wann", timestamp.ToString("g"));
            embed.AddInlineField("Klasse", character.Class);

            if (alts.Any())
                embed.AddField("Alt(s)", string.Join(Environment.NewLine, alts.Select(c => c.GetNameAndDescription())));

            if (character.Specializations.Any())
                embed.AddField("Spec(s)", string.Join(Environment.NewLine, character.Specializations.Select(s => s.GetDescription())));

            return embed.Build();
        }
    }
}
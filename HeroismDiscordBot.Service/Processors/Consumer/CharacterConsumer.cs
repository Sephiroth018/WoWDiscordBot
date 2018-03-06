using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using HeroismDiscordBot.Service.Entities;
using MoreLinq;

namespace HeroismDiscordBot.Service.Processors.Consumer
{
    public class CharacterConsumer
    {
        private readonly IGuild _discordGuild;

        public CharacterConsumer(IGuild discordGuild)
        {
            _discordGuild = discordGuild;
        }

        public void ConsumeLeftCharacter(Character character)
        {
            character.Left = DateTime.Now;
            SendDiscordMessage(character, "Gildenmitglied hat uns verlassen!", character.Left.Value, DiscordMessageType.Left);
        }

        public void ConsumeJoinedCharacter(Character character)
        {
            SendDiscordMessage(character, "Neuzugang! Willkommen!", character.Joined, DiscordMessageType.Joined);
        }

        private void SendDiscordMessage(Character character, string title, DateTime timestamp, DiscordMessageType messageType)
        {
            var channel = _discordGuild.GetTextChannelAsync(420312901157519362)
                                       .Result;

            List<Character> GetAlts(Character c)
            {
                return c.Player
                        .Characters
                        .Where(a => a.Name != c.Name && !a.Left.HasValue)
                        .OrderBy(a => !a.IsMain)
                        .ToList();
            }

            var alts = GetAlts(character);
            var embed = BuildPlayerChangedMessage(character, title, timestamp, messageType, alts);

            var message = channel.SendMessageAsync("", embed: embed)
                                 .Result;
            character.Messages.Add(new DiscordMessage
                                   {
                                       MessageId = (long) message.Id,
                                       ChannelId = (long) message.Channel.Id,
                                       DiscordMessageType = messageType
                                   });

            if (alts.Any())
                alts.AsParallel()
                    .ForEach(a =>
                             {
                                 while (!a.Messages.Any())
                                     Task.Delay(500)
                                         .Wait();
                                 var sentMessage = channel.GetMessageAsync((ulong) a.Messages.First(m => m.DiscordMessageType == DiscordMessageType.Joined)
                                                                                    .MessageId)
                                                          .Result as IUserMessage;
                                 sentMessage.ModifyAsync(m => { m.Embed = BuildPlayerChangedMessage(a, "Neuzugang! Willkommen!", a.Joined, DiscordMessageType.Joined, GetAlts(a)); });
                             });
        }

        private static Embed BuildPlayerChangedMessage(Character character, string title, DateTime timestamp, DiscordMessageType messageType, IReadOnlyCollection<Character> alts)
        {
            var embed = new EmbedBuilder();
            switch (messageType)
            {
                case DiscordMessageType.Joined:
                    embed.WithColor(Color.Green);
                    break;
                case DiscordMessageType.Left:
                    embed.WithColor(Color.Red);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            embed.Title = title;
            embed.AddInlineField("Wer", character.Name);
            embed.AddInlineField("Wann", timestamp.ToString("g"));
            embed.AddInlineField("Klasse", character.Class);

            if (alts.Any())
                embed.AddField("Alt(s)", string.Join(Environment.NewLine, alts.Select(c => $"{(c.IsMain ? "**Main: **" : string.Empty)}{c.Name} - {string.Join(", ", c.Specializations.Select(s => s.Role))}")));

            embed.AddField("Spec(s)", string.Join(Environment.NewLine, character.Specializations.Select(s => $"{s.Role} - {s.Name} ({s.ItemLevel})")));
            return embed.Build();
        }

        public void ConsumeExistingCharacter((CharacterData, Character Character) data)
        {
            var (characterData, character) = data;
            var specialization = character.Specializations.FirstOrDefault(s => s.Name == characterData.Specialization);

            if (specialization == null)
                character.Specializations.Add(new Specialization
                                              {
                                                  Name = characterData.Specialization,
                                                  ItemLevel = characterData.SpecializationLevel
                                              });
            else
                specialization.ItemLevel = characterData.SpecializationLevel;

            var main = character.Player
                                .Characters
                                .GroupBy(c => c.Rank)
                                .OrderBy(c => c.Key)
                                .ToList();

            character.Player.Characters.ForEach(c => c.IsMain = false);
            if (main.First().Count() == 1)
            {
                main.First()
                    .First()
                    .IsMain = true;
            }

            character.LastUpdate = DateTime.Now;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities;

namespace HeroismDiscordBot.Service.Consumer
{
    public class CharacterDiscordMessageConsumer : IConsumer<Character, List<Character>>
    {
        private const string MemberJoinedTitle = "Neuzugang! Willkommen!";
        private const string MemberLeftTitle = "Gildenmitglied hat uns verlassen!";
        private readonly IConfiguration _configuration;
        private readonly IDiscordFactory _discordclient;
        private readonly Func<IRepository> _repositoryFactory;

        public CharacterDiscordMessageConsumer(IDiscordFactory discordclient, IConfiguration configuration, Func<IRepository> repositoryFactory)
        {
            _discordclient = discordclient;
            _configuration = configuration;
            _repositoryFactory = repositoryFactory;
        }

        public List<Character> Consume(Character character)
        {
            using (var repository = _repositoryFactory.Invoke())
            {
                try
                {
                    repository.Characters.Attach(character);
                }
                catch (Exception)
                {
                    return new List<Character>();
                }

                var channel = _discordclient.GetGuild()
                                            .GetTextChannelAsync(_configuration.DiscordMemberChangeChannelId)
                                            .Result;

                var alts = GetAlts(character);

                if (!character.DiscordMessages.Any())
                {
                    var (messageId, channelId) = SendDiscordMessage(character, MemberJoinedTitle, character.Joined, Color.Green, channel, alts);
                    character.DiscordMessages.Add(new CharacterDiscordMessage
                                                  {
                                                      ChannelId = (long)channelId,
                                                      MessageId = (long)messageId,
                                                      DiscordMessageType = DiscordMessageType.Joined
                                                  });
                }
                else if (character.Left.HasValue)
                {
                    var (messageId, channelId) = SendDiscordMessage(character, MemberLeftTitle, character.Left.Value, Color.Red, channel, alts);
                    character.DiscordMessages.Add(new CharacterDiscordMessage
                                                  {
                                                      ChannelId = (long)channelId,
                                                      MessageId = (long)messageId,
                                                      DiscordMessageType = DiscordMessageType.Joined
                                                  });
                }
                else
                {
                    UpdateMessage(character, channel);
                }

                repository.SaveChanges();

                return alts;
            }
        }

        protected (ulong messageId, ulong channelId) SendDiscordMessage(Character character, string title, DateTime timestamp, Color color, ITextChannel channel, List<Character> alts)
        {
            var embed = BuildPlayerChangedMessage(character, title, timestamp, color, alts);

            var message = channel.SendMessageAsync("", embed: embed)
                                 .Result;

            return (message.Id, message.Channel.Id);
        }

        private static void UpdateMessage(Character character, IMessageChannel channel)
        {
            var sentMessage = channel.GetMessageAsync((ulong)character.DiscordMessages.First(m => m.DiscordMessageType == DiscordMessageType.Joined)
                                                                      .MessageId)
                                     .Result as IUserMessage;
            sentMessage.ModifyAsync(m => { m.Embed = BuildPlayerChangedMessage(character, MemberJoinedTitle, character.Joined, Color.Green, GetAlts(character)); }).Wait();
        }

        private static List<Character> GetAlts(Character c)
        {
            return c.Player.Characters.Where(a => a.Name != c.Name && !a.Left.HasValue)
                    .OrderBy(a => !a.IsMain)
                    .ToList();
        }

        private static Embed BuildPlayerChangedMessage(Character character, string title, DateTime timestamp, Color color, IReadOnlyCollection<Character> alts)
        {
            var embed = new EmbedBuilder { Title = title };

            embed.WithColor(color);

            embed.AddInlineField("Wer", character.Name);
            embed.AddInlineField("Wann", timestamp.ToString("g"));
            embed.AddInlineField("Klasse", character.Class);

            if (alts.Any())
                embed.AddField("Alt(s)", string.Join(Environment.NewLine, alts.Select(c => $"{(c.IsMain ? "**Main: **" : string.Empty)}{c.Name} - {string.Join(", ", c.Specializations.Select(s => s.Role))}")));

            if (character.Specializations.Any())
                embed.AddField("Spec(s)", string.Join(Environment.NewLine, character.Specializations.Select(s => $"{s.Role} - {s.Name} ({s.ItemLevel})")));

            return embed.Build();
        }
    }
}
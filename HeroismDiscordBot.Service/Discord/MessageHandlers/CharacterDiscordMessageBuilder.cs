using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using HeroismDiscordBot.Service.Common;
using HeroismDiscordBot.Service.Entities.DAL;

namespace HeroismDiscordBot.Service.Discord.MessageHandlers
{
    public class CharacterDiscordMessageBuilder : IDiscordMessageBuilder<CharacterDiscordMessage>, IDiscordMessageSender<CharacterDiscordMessage>
    {
        private const string MemberJoinedTitle = "Neuzugang! Willkommen!";
        private const string MemberLeftTitle = "Gildenmitglied hat uns verlassen!";
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _discordClient;

        public CharacterDiscordMessageBuilder(DiscordSocketClient discordClient, IConfiguration configuration)
        {
            _discordClient = discordClient;
            _configuration = configuration;
        }

        public Embed BuildMessage(CharacterDiscordMessage data)
        {
            var character = data.GuildMembershipState.Character;
            var alts = GetAlts(character);
            var embed = new EmbedBuilder();

            if (data.GuildMembershipState.State == GuildMemberState.Joined)
            {
                embed.Title = MemberJoinedTitle;
                embed.Color = Color.Green;
            }
            else
            {
                embed.Title = MemberLeftTitle;
                embed.Color = Color.Red;
            }

            //TODO add status field, make parameter timestamp optional and use lastchanged if not set (with according text)
            embed.AddInlineField("Wer", character.Name);
            embed.AddInlineField("Wann", data.GuildMembershipState.Timestamp.ToString("g"));
            embed.AddInlineField("Klasse", character.Class);

            if (character.Specializations.Any())
                embed.AddField("Spec(s)", string.Join(Environment.NewLine, character.Specializations.Select(s => s.GetDescription())));

            if (alts.Any())
                embed.AddField("Alt(s)", string.Join(Environment.NewLine, alts.Select(c => c.GetNameAndDescription())));

            return embed.Build();
        }

        public void SendMessage(CharacterDiscordMessage data)
        {
            var messageData = BuildMessage(data);
            var guild = _discordClient.GetGuild(_configuration.DiscordGuildId) as IGuild;
            var channel = guild.GetTextChannelAsync(_configuration.DiscordMemberChangeChannelId)
                               .Result;

            if (data.ChannelId == 0 || data.MessageId == 0)
            {
                var discordMessage = channel.SendMessageAsync("", embed: messageData)
                                            .Result;
                data.MessageId = (long)discordMessage.Id;
                data.ChannelId = (long)discordMessage.Channel.Id;
            }
            else
            {
                var sentMessage = channel.GetMessageAsync((ulong)data.MessageId)
                                         .Result as IUserMessage;

                sentMessage?.ModifyAsync(m => m.Embed = messageData).Wait();
            }
        }

        private static List<Character> GetAlts(Character character)
        {
            return character.Player.Characters.Where(a => a.Name != character.Name && !a.Left.HasValue)
                            .OrderBy(a => !a.IsMain)
                            .ToList();
        }
    }
}
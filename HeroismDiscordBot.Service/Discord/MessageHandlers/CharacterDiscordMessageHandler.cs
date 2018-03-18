using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using HeroismDiscordBot.Service.Common.Configuration;
using HeroismDiscordBot.Service.Entities.DAL;
using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Discord.MessageHandlers
{
    [UsedImplicitly]
    public class CharacterDiscordMessageHandler : IDiscordMessageBuilder<CharacterDiscordMessage>, IDiscordMessageSender<CharacterDiscordMessage>, IDiscordMessageBuilder<Character>
    {
        private const string MemberJoinedTitle = "Neuzugang! Willkommen!";
        private const string MemberLeftTitle = "Gildenmitglied hat uns verlassen!";

        private static readonly Dictionary<GuildMemberState, string> MemberStateTranslations = new Dictionary<GuildMemberState, string>
                                                                                               {
                                                                                                   { GuildMemberState.Joined, "Gildenmitglied" },
                                                                                                   { GuildMemberState.Left, "Nicht in der Gilde" }
                                                                                               };

        private readonly IDiscordConfiguration _configuration;
        private readonly DiscordSocketClient _discordClient;
        private readonly IDiscorMemberConfiguration _memberConfiguration;

        public CharacterDiscordMessageHandler(DiscordSocketClient discordClient, IDiscordConfiguration configuration, IDiscorMemberConfiguration memberConfiguration)
        {
            _discordClient = discordClient;
            _configuration = configuration;
            _memberConfiguration = memberConfiguration;
        }

        public Embed BuildMessage(Character character)
        {
            return BuildMessageInternal(character, true);
        }

        public Embed BuildMessage(CharacterDiscordMessage data)
        {
            return BuildMessageInternal(data.GuildMembershipState.Character, false);
        }

        public void SendMessage(CharacterDiscordMessage data)
        {
            var messageData = BuildMessage(data);
            var guild = _discordClient.GetGuild(_configuration.GuildId) as IGuild;
            var channel = guild.GetTextChannelAsync(_memberConfiguration.NotificationChannelId)
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

        public IDisposable EnterTypingState()
        {
            var guild = _discordClient.GetGuild(_configuration.GuildId) as IGuild;
            var channel = guild.GetTextChannelAsync(_memberConfiguration.NotificationChannelId)
                               .Result;

            return channel.EnterTypingState();
        }

        private List<Character> GetAlts(Character character)
        {
            return character.Player.Characters.Where(a => a.Name != character.Name && (_memberConfiguration.ShowNonGuildAlts || a.CurrentMembershipState.State != GuildMemberState.Left))
                            .OrderBy(a => !a.IsMain)
                            .ToList();
        }

        private Embed BuildMessageInternal(Character character, bool showStatus)
        {
            var alts = GetAlts(character);
            var embed = new EmbedBuilder();

            if (character.CurrentMembershipState.State == GuildMemberState.Joined)
            {
                embed.Title = MemberJoinedTitle;
                embed.Color = _memberConfiguration.MemberJoinedColor.ToDiscordColor();
            }
            else
            {
                embed.Title = MemberLeftTitle;
                embed.Color = _memberConfiguration.MemberLeftColor.ToDiscordColor();
            }

            embed.AddInlineField("Wer", character.Name);
            if (showStatus)
                embed.AddInlineField("Status", MemberStateTranslations[character.CurrentMembershipState.State]);
            embed.AddInlineField("Wann", character.CurrentMembershipState.Timestamp.ToLocalTime().ToString("g"));
            embed.AddInlineField("Letzte Aktualisierung", character.LastUpdate.ToLocalTime().ToString("g"));
            embed.AddInlineField("Klasse", character.Class);

            if (character.Specializations.Any())
                embed.AddField("Spec(s)", string.Join(Environment.NewLine, character.Specializations.Select(s => s.GetDescription())));

            if (alts.Any())
                embed.AddField("Alt(s)", string.Join(Environment.NewLine, alts.Select(c => c.GetNameAndDescription())));

            return embed.Build();
        }
    }
}
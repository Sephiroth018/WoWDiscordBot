using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Rest;
using HeroismDiscordBot.Service.Entities;
using HeroismDiscordBot.Service.Properties;
using MoreLinq;
using WowDotNetAPI;
using WowDotNetAPI.Models;
using Character = HeroismDiscordBot.Service.Entities.Character;

namespace HeroismDiscordBot.Service.Processors
{
    public class GuildMemberProcessor
    {
        private readonly WowExplorer _wowClient;
        private readonly Database _database;
        private readonly RestGuild _discordGuild;

        public GuildMemberProcessor(WowExplorer wowClient, Database database, RestGuild discordGuild)
        {
            _wowClient = wowClient;
            _database = database;
            _discordGuild = discordGuild;
        }

        public void DoWork()
        {
            var characters = GetCharacters();
            UpdateSpecializationData(characters);
            HandleLeftCharacters(characters);
            HandleJoinedCharacters(characters);

            //_database.SaveChanges();
        }

        private List<QueriedAndStoredCharacterData> GetCharacters()
        {
            var guild = _wowClient.GetGuild(Region.EU, Settings.Default.WoWRealm, Settings.Default.WoWGuild, GuildOptions.GetEverything);
            var queriedCharacters = guild.Members
                                         .Take(1)
                                         .Select(NormalizeCharacterData)
                                         .ToList();
            var storedCharacters = _database.Characters.ToList();

            var characters =
                queriedCharacters.FullJoin(storedCharacters,
                                           c => c.Name,
                                           c => c.Name,
                                           c => new QueriedAndStoredCharacterData {Queried = c, Stored = null},
                                           c => new QueriedAndStoredCharacterData {Queried = null, Stored = c},
                                           (qc, sc) => new QueriedAndStoredCharacterData {Queried = qc, Stored = sc})
                                 .ToList();

            return characters;
        }

        private void HandleJoinedCharacters(IEnumerable<QueriedAndStoredCharacterData> characters)
        {
            characters.Where(c => c.Stored == null)
                      .ForEach(c =>
                               {
                                   c.Stored = new Character
                                              {
                                                  Joined = DateTime.Now,
                                                  Name = c.Queried.Name,
                                                  Level = c.Queried.Level,
                                                  Class = c.Queried.Class,
                                                  LastUpdate = DateTime.Now,
                                                  Specializations = new List<Specialization>
                                                                    {
                                                                        new Specialization
                                                                        {
                                                                            ItemLevel = c.Queried.SpecializationLevel,
                                                                            Name = c.Queried.Specialization
                                                                        }
                                                                    }
                                              };

                                   _database.Characters.Add(c.Stored);
                                   SendDiscordMessage(c.Stored);
                               });
        }

        private void SendDiscordMessage(Character character)
        {
            var channel = _discordGuild.GetTextChannelAsync(420312901157519362).Result;
            channel.SendMessageAsync($"TestMessage for character {character.Name}").Wait();
        }

        private void UpdateSpecializationData(IEnumerable<QueriedAndStoredCharacterData> characters)
        {
            characters.Where(c => c.Stored != null)
                      .ForEach(c =>
                               {
                                   var specialization = c.Stored.Specializations.FirstOrDefault(s => s.Name == c.Queried.Specialization);

                                   if (specialization == null)
                                       c.Stored.Specializations.Add(new Specialization
                                                                    {
                                                                        Name = c.Queried.Specialization,
                                                                        ItemLevel = c.Queried.SpecializationLevel
                                                                    });
                                   else
                                       specialization.ItemLevel = c.Queried.SpecializationLevel;

                                   c.Stored.LastUpdate = DateTime.Now;
                               });
        }

        private void HandleLeftCharacters(IEnumerable<QueriedAndStoredCharacterData> characters)
        {
            characters.Where(c => c.Queried == null)
                      .ForEach(c => { c.Stored.Left = DateTime.Now; });
        }

        private CharacterData NormalizeCharacterData(GuildMember member)
        {
            var characterInfo = _wowClient.GetCharacter(Region.EU, Settings.Default.WoWRealm, member.Character.Name, CharacterOptions.GetItems | CharacterOptions.GetAchievements | CharacterOptions.GetPets);
            var result = new CharacterData
                         {
                             Name = characterInfo.Name,
                             Level = characterInfo.Level,
                             Class = characterInfo.Class.ToString()
                                                  .First()
                                                  .ToString()
                                                  .ToUpper() +
                                     characterInfo.Class.ToString()
                                                  .Substring(1),
                             Specialization = member.Character.Specialization.Role,
                             SpecializationLevel = characterInfo.Items.AverageItemLevelEquipped,
                             //AchievementPoints = characterInfo.AchievementPoints,
                             //Pets = characterInfo.Pets.NumCollected;
                         };

            return result;
        }
    }

    internal class QueriedAndStoredCharacterData
    {
        public CharacterData Queried { get; set; }
        public Character Stored { get; set; }
    }

    internal class CharacterData
    {
        public int SpecializationLevel { get; set; }
        public string Specialization { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
    }
}
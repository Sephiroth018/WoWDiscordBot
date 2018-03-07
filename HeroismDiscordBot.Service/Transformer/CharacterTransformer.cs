using System;
using System.Collections.Generic;
using System.Linq;
using HeroismDiscordBot.Service.Entities;
using HeroismDiscordBot.Service.Processors;
using MoreLinq;

namespace HeroismDiscordBot.Service.Transformer
{
    public class CharacterTransformer : ITransformer<(CharacterData, Character), Character>
    {
        private readonly Func<IRepository> _repositoryFactory;

        public CharacterTransformer(Func<IRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public Character Transform((CharacterData, Character) data)
        {
            var (characterData, character) = data;

            using (var repository = _repositoryFactory.Invoke())
            {
                if (character == null)
                {
                    var existingCharacter = repository.Characters
                                                      .ToList()
                                                      .Union(repository.Characters.Local)
                                                      .FirstOrDefault(ec => ec.AchievementPoints == characterData.AchievementPoints && ec.AchievementsHash == characterData.AchievementsHash && ec.PetsHash == characterData.PetsHash);
                    character = new Character
                                {
                                    Joined = DateTime.Now,
                                    Name = characterData.Name,
                                    Class = characterData.Class,
                                    Specializations = new List<Specialization>(),
                                    Player = existingCharacter?.Player ?? new Player()
                                };

                    repository.Characters.Add(character);
                }
                else
                {
                    repository.Characters.Attach(character);
                }

                character.Level = characterData.Level;
                character.LastUpdate = DateTime.Now;

                if(!string.IsNullOrWhiteSpace(characterData.Specialization))
                {
                    var specialization = character.Specializations.FirstOrDefault(s => s.Name == characterData.Specialization);

                    if (specialization == null)
                        character.Specializations.Add(new Specialization
                                                      {
                                                          Name = characterData.Specialization,
                                                          ItemLevel = characterData.SpecializationLevel,
                                                          Role = characterData.SpecializationRole
                                                      });
                    else
                        specialization.ItemLevel = characterData.SpecializationLevel;
                }

                character.AchievementPoints = characterData.AchievementPoints;
                character.AchievementsHash = characterData.AchievementsHash;
                character.PetsHash = characterData.PetsHash;
                character.Rank = characterData.Rank;

                var main = character.Player
                                    .Characters
                                    .GroupBy(c => c.Rank)
                                    .OrderBy(c => c.Key)
                                    .ToList();

                character.Player.Characters.ForEach(c => c.IsMain = false);
                if (main.First().Count() == 1)
                    main.First()
                        .First()
                        .IsMain = true;

                repository.SaveChanges();
            }

            return character;
        }
    }
}
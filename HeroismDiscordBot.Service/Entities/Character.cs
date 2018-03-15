using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroismDiscordBot.Service.Entities
{
    public class Character : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Joined { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();

        public string Class { get; set; }

        public int Level { get; set; }

        public DateTime? Left { get; set; }

        public DateTime LastUpdate { get; set; }

        public virtual Player Player { get; set; }

        public int AchievementPoints { get; set; }

        public string AchievementsHash { get; set; }

        public string PetsHash { get; set; }

        public int? Rank { get; set; }

        public virtual ICollection<CharacterDiscordMessage> DiscordMessages { get; set; } = new List<CharacterDiscordMessage>();

        public bool IsMain { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        public string GetNameAndDescription()
        {
            var result = $"{(IsMain ? "**Main: **" : Left.HasValue ? "**Nicht in Gilde: **" : string.Empty)}{Name}: {Class}";

            if (Specializations.Any())
                result = $"{result} {string.Join(",", Specializations.Select(s => s.GetDescription()))}";

            return result;
        }

        public List<Character> GetAlts()
        {
            return Player.Characters.Where(a => a.Name != Name)
                         .OrderBy(a => !a.IsMain)
                         .ToList();
        }
    }
}
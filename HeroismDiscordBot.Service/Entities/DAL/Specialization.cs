using JetBrains.Annotations;

namespace HeroismDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly]
    public class Specialization : BaseEntity
    {
        public string Name { get; set; }

        public int ItemLevel { get; set; }

        public string Role { get; set; }

        public virtual Character Character { get; set; }

        public string GetDescription()
        {
            return $"{Name}-{Role} ({ItemLevel})";
        }
    }
}
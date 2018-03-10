namespace HeroismDiscordBot.Service.Entities
{
    public class Specialization : BaseEntity
    {
        public string Name { get; set; }

        public int ItemLevel { get; set; }

        public string Role { get; set; }

        public virtual Character Character { get; set; }
    }
}
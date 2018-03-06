using System.ComponentModel.DataAnnotations;

namespace HeroismDiscordBot.Service.Entities
{
    public class Specialization
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemLevel { get; set; }
    }
}
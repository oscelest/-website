using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG
{
    public class ParadigmSkill
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public required ParadigmUnit ParadigmUnit { get; set; }

        public required Skill Skill { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}

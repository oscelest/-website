using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG
{
    public class ParadigmUnit
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public required Paradigm Paradigm { get; set; }
        
        public required Unit Unit { get; set; }

        public List<ParadigmSkill> ParadigmSkillList { get; set; } = new();

        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}

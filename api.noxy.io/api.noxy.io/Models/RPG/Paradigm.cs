using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG
{
    public class Paradigm
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }

        public required Guild Guild { get; set; }

        public List<ParadigmUnit> ParadigmUnitList { get; set; } = new();

        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}

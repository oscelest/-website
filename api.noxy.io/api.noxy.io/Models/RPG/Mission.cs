using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG
{
    public class Mission
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required TemplateMission TemplateMission { get; set; }

        [Required]
        public required Guild Guild { get; set; }

        public List<Unit> UnitList { get; set; } = new();

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        public DateTime TimeInitiated { get; set; }

        #region -- Mappings --

        public List<TemplateRole> TemplateRoleList { get; set; } = new();

        #endregion -- Mappings --
    }
}

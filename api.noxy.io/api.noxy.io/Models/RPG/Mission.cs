using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Models.Game.Unit;
using api.noxy.io.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Mission
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Template.Mission TemplateMission { get; set; }

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        // Mappings

        public List<Template.Role> TemplateRoleList { get; set; } = new();

        // Implementations

        [Table("MissionInitiated")]
        public class Initiated : Mission
        {
            [Required]
            public DateTime TimeInitiated { get; set; } = DateTime.UtcNow;

            // Mappings

            public List<Unit.Initiated> UnitList { get; set; } = new();
        }

        [Table("MissionAvailable")]
        public class Available : Mission
        {

        }
    }
}

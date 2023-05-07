using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{

    public class Unit
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required TemplateUnit TemplateUnit { get; set; }

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Experience { get; set; } = 0;

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        public DateTime? TimeInitiated { get; set; }

        #region -- Mappings --

        public List<Role> RoleList { get; set; } = new();

        #endregion -- Mappings --
    }
}

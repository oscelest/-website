using api.noxy.io.Models.Game.Feat;
using api.noxy.io.Models.Game.Role;
using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Modifier
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required float Value { get; set; }

        [Required]
        [Column(TypeName = "varchar(32)")]
        public required ArithmeticalTagType ArithmeticalTag { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        // Implementations

        public class Attribute : Modifier
        {
            [Required]
            [Column(nameof(MissionTag), TypeName = "varchar(32)")]
            public required AttributeTagType AttributeTag { get; set; }
        }

        public class Guild : Modifier
        {
            // Mappings

            [Required]
            public required List<Template.Feat> FeatList { get; set; }

            // Implementations

            public class Mission : Guild
            {
                [Required]
                [Column(nameof(MissionTag), TypeName = "varchar(32)")]
                public required ModifierGuildMissionTagType MissionTag { get; set; }
            }

            public class Role : Guild
            {
                [Required]
                [Column(nameof(RoleTag), TypeName = "varchar(32)")]
                public required ModifierGuildRoleTagType RoleTag { get; set; }

                public RoleTypeEntity? RoleType { get; set; }
            }

            public class Unit : Guild
            {
                [Required]
                [Column(nameof(UnitTag), TypeName = "varchar(32)")]
                public required ModifierGuildUnitTagType UnitTag { get; set; }
            }
        }

    }
}

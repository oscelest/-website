using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Unit")]
    public class UnitEntity : SingleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public required string Name { get; set; } 
        [Required]
        [DefaultValue(0)]
        public required int Experience { get; set; } 

        [Required]
        [DefaultValue(false)]
        public required bool Recruited { get; set; }

        [Required]
        public required GuildEntity Guild { get; set; }

        [Required]
        public required List<RoleLevelEntity> RoleLevelList { get; set; } = new();

        public int GetCost()
        {
            // TODO: Implement cost calcualtion based on role level list values
            return 0;
        }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }
            public int Experience { get; set; }
            public bool Recruited { get; set; }
            public IEnumerable<RoleLevelEntity.DTO> RoleLevelList { get; set; } 

            public DTO(UnitEntity entity) : base(entity)
            {
                Name = entity.Name;
                Experience = entity.Experience;
                Recruited = entity.Recruited;
                RoleLevelList = entity.RoleLevelList.Select(x => x.ToDTO());
            }
        }

        #endregion -- DTO --

    }
}

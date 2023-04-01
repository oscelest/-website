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
        public string Name { get; set; } = string.Empty;

        [Required]
        [DefaultValue(0)]
        public int Experience { get; set; } = 0;

        [Required]
        [DefaultValue(false)]
        public bool Recruited { get; set; } = false;

        [Required]
        public Guid GuildID { get; set; }

        [Required]
        public List<RoleLevelEntity> RoleLevelList { get; set; } = new();

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

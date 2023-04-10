using api.noxy.io.Models.Game.Item;
using api.noxy.io.Models.Game.Unit;
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
        public required UnitTypeEntity UnitType { get; set; }

        public List<UnitRoleEntity> UnitRoleList { get; set; } = new();

        public int GetCost()
        {
            // TODO: Implement cost calcualtion based on role level list values
            return 0;
        }

        public bool HasEquipmentSlot(EquipmentSlotEntity entityEquipmentSlot)
        {
            return UnitType.EquipmentSlotList.Any(x => x.ID == entityEquipmentSlot.ID);
        }

        #region -- Mapping --

        public List<GuildUnitEntity> GuildUnitList { get; set; } = new();

        #endregion -- Mapping --

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }
            public int Experience { get; set; }
            public bool Recruited { get; set; }
            public IEnumerable<UnitRoleEntity.DTO> RoleLevelList { get; set; }

            public DTO(UnitEntity entity) : base(entity)
            {
                Name = entity.Name;
                Experience = entity.Experience;
                Recruited = entity.Recruited;
                RoleLevelList = entity.UnitRoleList.Select(x => x.ToDTO());
            }
        }

        #endregion -- DTO --

    }
}

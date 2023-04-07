using api.noxy.io.Models.Game.Item;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("UnitBase")]
    public class UnitEntity : SingleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public required string Name { get; set; }

        [Required]
        [Comment("The list of equipment slots available to a unit with this unit type.")]
        public List<EquipmentSlotEntity> EquipmentSlotList { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }
            public IEnumerable<EquipmentSlotEntity.DTO> EquipmentSlotList { get; set; }

            public DTO(UnitEntity entity) : base(entity)
            {
                Name = entity.Name;
                EquipmentSlotList = entity.EquipmentSlotList.Select(x => x.ToDTO());
            }
        }

        #endregion -- DTO --

    }
}

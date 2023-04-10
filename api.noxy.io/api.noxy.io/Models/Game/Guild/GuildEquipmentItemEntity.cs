using api.noxy.io.Models.Game.Item;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.Game.Guild
{
    public class GuildEquipmentItemEntity : GuildItemEntity<EquipmentItemEntity>
    {
        [Required]
        public GuildUnitEntity? Unit { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : GuildItemEntity<EquipmentItemEntity>.DTO
        {
            public GuildUnitEntity.DTO? Unit { get; set; }
            new public EquipmentItemEntity.DTO Item { get; set; }

            public DTO(GuildEquipmentItemEntity entity) : base(entity)
            {
                Unit = entity.Unit?.ToDTO();
                Item = entity.Item.ToDTO();
            }
        }

        #endregion -- DTO --
    }
}

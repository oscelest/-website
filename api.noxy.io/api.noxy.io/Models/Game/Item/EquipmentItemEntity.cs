using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Models.Game.Item
{
    [Index(nameof(Name), IsUnique = true)]
    public class EquipmentItemEntity : ItemEntity
    {
        public required EquipmentSlotEntity Slot { get; set; }

        public required List<ItemSocketEntity> ItemSocketList { get; set; } = new List<ItemSocketEntity>();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }

            public DTO(ItemEntity entity) : base(entity)
            {
                Name = entity.Name;
            }
        }

        #endregion -- DTO --

    }
}

using api.noxy.io.Models.Game.Role;
using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Models.Game.Item
{
    [Index(nameof(Name), IsUnique = true)]
    public class MaterialItemEntity : ItemEntity
    {
        public required RoleTypeEntity RoleType { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public RoleTypeEntity.DTO RoleType { get; set; }

            public DTO(MaterialItemEntity entity) : base(entity)
            {
                RoleType = entity.RoleType.ToDTO();
            }
        }

        #endregion -- DTO --

    }
}

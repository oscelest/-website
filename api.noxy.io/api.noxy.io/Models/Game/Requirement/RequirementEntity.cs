using api.noxy.io.Models.Game.Feat;
using api.noxy.io.Models.Game.Guild;

namespace api.noxy.io.Models.Game.Requirement
{
    public class RequirementEntity : SingleEntity
    {
        #region -- Mapping --

        public List<FeatEntity> FeatList { get; set; } = new();
        public List<GuildRoleEntity> RoleList { get; set; } = new();

        #endregion -- Mapping --

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public DTO(RequirementEntity entity) : base(entity)
            {

            }
        }

        #endregion -- DTO --

    }
}

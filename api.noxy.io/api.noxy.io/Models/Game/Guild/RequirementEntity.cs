namespace api.noxy.io.Models.Game.Guild
{
    public class RequirementEntity : SingleEntity
    {
        #region -- Mapping --

        public List<FeatEntity> FeatList { get; set; } = new();
        public List<RoleEntity> RoleList { get; set; } = new();

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

namespace api.noxy.io.Models.Game.Guild
{
    public class RequirementEntity : SingleEntity
    {
        public List<FeatEntity> FeatList { get; set; } = new();
        public List<RoleEntity> RoleList { get; set; } = new();

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

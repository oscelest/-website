using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("FeatRequirement")]
    public abstract class FeatRequirementEntity : SimpleEntity
    {
        // Inverse
        public FeatEntity Feat { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public DTO(FeatRequirementEntity entity) : base(entity)
            {

            }
        }

        #endregion -- DTO --

    }
}


namespace api.noxy.io.Models
{
    public abstract class JunctionEntity : Entity
    {

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : Entity.DTO
        {
            public DTO(JunctionEntity entity) : base(entity)
            {

            }
        }

        #endregion -- DTO --

    }
}

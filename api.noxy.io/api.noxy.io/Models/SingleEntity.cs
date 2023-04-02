namespace api.noxy.io.Models
{
    public abstract class SingleEntity : Entity
    {
        public DateTime? TimeUpdated { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : Entity.DTO
        {
            public DateTime? TimeUpdated { get; set; }

            public DTO(SingleEntity entity) : base(entity)
            {
                TimeUpdated = entity.TimeUpdated;
            }
        }

        #endregion -- DTO --

    }
}

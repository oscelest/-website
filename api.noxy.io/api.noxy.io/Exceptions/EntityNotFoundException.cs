using api.noxy.io.Models;

namespace api.noxy.io.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Guid UserID { get; private set; }
        public SingleEntity? ParentEntity { get; private set; }

        public EntityNotFoundException(Guid userID, SingleEntity? parent = null)
        {
            UserID = userID;
            ParentEntity = parent;
        }
    }
}

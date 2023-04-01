using api.noxy.io.Models;

namespace api.noxy.io.Exceptions
{
    public class EntityStateException : Exception
    {
        public Guid UserID { get; private set; }
        public SingleEntity Entity { get; private set; }

        public EntityStateException(Guid userID, SingleEntity entity)
        {
            UserID = userID;
            Entity = entity;
        }
    }
}

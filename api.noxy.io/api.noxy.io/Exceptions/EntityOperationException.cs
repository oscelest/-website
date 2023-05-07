using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Exceptions
{
    public class EntityOperationException<T> : Exception where T : class
    {
        public EntityOperationException() : base()
        {
        }

        public EntityOperationException(string message) : base(message)
        {
        }
    }
}

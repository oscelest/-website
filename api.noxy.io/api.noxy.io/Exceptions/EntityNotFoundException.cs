using api.noxy.io.Models;

namespace api.noxy.io.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}

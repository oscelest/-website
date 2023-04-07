using api.noxy.io.Models;

namespace api.noxy.io.Exceptions
{
    public class EntityStateException : Exception
    {
        public EntityStateException()
        {

        }

        public EntityStateException(string message) : base(message)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Exceptions
{
    public class EntityNotFoundException<T> : Exception where T : class
    {
        public object Identifier { get; set; }

        public EntityNotFoundException(object identifier) : base()
        {
            Identifier = identifier;
        }

        public EntityNotFoundException(string message, object identifier) : base(message)
        {
            Identifier = identifier;
        }
    }
}

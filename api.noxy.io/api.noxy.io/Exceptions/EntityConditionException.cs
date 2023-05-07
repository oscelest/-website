using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Exceptions
{
    public class EntityConditionException<T> : Exception where T : class
    {
        public object Identifier { get; set; }

        public EntityConditionException(object identifier) : base()
        {
            Identifier = identifier;
        }

        public EntityConditionException(string message, object identifier) : base(message)
        {
            Identifier = identifier;
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Exceptions
{
    public class EntityConditionException<T> : Exception where T : class
    {
        public DbSet<T> DBSet { get; set; }
        public object Identifier { get; set; }

        public EntityConditionException(DbSet<T> set, object identifier) : base()
        {
            DBSet = set;
            Identifier = identifier;
        }

        public EntityConditionException(string message, DbSet<T> set, object identifier) : base(message)
        {
            DBSet = set;
            Identifier = identifier;
        }
    }
}

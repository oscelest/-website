using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Exceptions
{
    public class EntityNotFoundException<T> : Exception where T : class
    {
        public DbSet<T> DBSet { get; set; }
        public object Identifier { get; set; }

        public EntityNotFoundException(DbSet<T> set, object identifier) : base()
        {
            DBSet = set;
            Identifier = identifier;
        }

        public EntityNotFoundException(string message, DbSet<T> set, object identifier) : base(message)
        {
            DBSet = set;
            Identifier = identifier;
        }
    }
}

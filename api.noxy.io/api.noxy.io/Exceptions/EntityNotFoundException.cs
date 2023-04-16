using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Exceptions
{
    public class EntityNotFoundException<T> : Exception where T : class
    {
        public DbSet<T> DBSet { get; set; }
        public Guid ID { get; set; }

        public EntityNotFoundException(DbSet<T> set, Guid id) : base()
        {
            DBSet = set;
            ID = id;
        }

        public EntityNotFoundException(string message, DbSet<T> set, Guid id) : base(message)
        {
            DBSet = set;
            ID = id;
        }
    }
}

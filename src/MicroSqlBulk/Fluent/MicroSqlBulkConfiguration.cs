
namespace MicroSqlBulk.Fluent
{
    public class MicroSqlBulkConfiguration
    {
        public MicroSqlBulkConfiguration Entity<TEntity>()
        {
            return this;
        }

        public MicroSqlBulkConfiguration Table(string name)
        {
            return this;
        }

        public MicroSqlBulkConfiguration Schema(string name)
        {
            return this;
        }

        public MicroSqlBulkConfiguration Timeout(int timeout)
        {
            return this;
        }
    }
}

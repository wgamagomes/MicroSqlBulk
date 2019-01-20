
namespace MicroSqlBulk.Fluent
{
    public static class MicroSqlBulkBuilder
    {
        public static MicroSqlBulkConfiguration Confugure<TEntity>()
        {
            return new MicroSqlBulkConfiguration().Entity<TEntity>();
        }
    }
}

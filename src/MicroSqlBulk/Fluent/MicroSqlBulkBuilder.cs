
namespace MicroSqlBulk.Fluent
{
    public static class MicroSqlBulkBuilder
    {

        public static FluentConfiguration Confugure<TEntity>()
        {
            return new FluentConfiguration().Entity<TEntity>();
        }
    }
}

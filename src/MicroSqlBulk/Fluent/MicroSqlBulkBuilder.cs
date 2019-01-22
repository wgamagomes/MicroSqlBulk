
namespace MicroSqlBulk.Fluent
{
    public static class MicroSqlBulkBuilder
    {

        public static FluentConfiguration Configure<TEntity>()
        {
            return new FluentConfiguration().Entity<TEntity>();
        }
    }
}

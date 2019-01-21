using System.Collections.Concurrent;

namespace MicroSqlBulk.Helper
{
    public static class CacheHelper
    {
        private static ConcurrentDictionary<string, SqlBulkConfiguration> _mapperCache = new ConcurrentDictionary<string, SqlBulkConfiguration>();

        public static SqlBulkConfiguration GetSqlBulkEntityConfiguration<TEntity>()
        {
            SqlBulkConfiguration sqlBulkEntityConfiguration;
            var nameOfT = typeof(TEntity).Name;

            if (!_mapperCache.TryGetValue(nameOfT, out sqlBulkEntityConfiguration))
            {
                var columns = ColumnHelper.GetFildesInfo<TEntity>();
                var tableName = TableHelper.GetTableName<TEntity>();
                sqlBulkEntityConfiguration = new SqlBulkConfiguration(columns, tableName);
                _mapperCache.TryAdd(nameOfT, sqlBulkEntityConfiguration);
            }

            return sqlBulkEntityConfiguration;
        }

    }
}

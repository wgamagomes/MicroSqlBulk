using System.Collections.Concurrent;

namespace MicroSqlBulk.Helper
{
    public static class CacheHelper
    {
        private static ConcurrentDictionary<string, TableInfo > _metadataCache = new ConcurrentDictionary<string, TableInfo >();

        public static TableInfo  GetTableInfo<TEntity>()
        {
            TableInfo  tableMetadata;
            var nameOfT = typeof(TEntity).Name;

            if (!_metadataCache.TryGetValue(nameOfT, out tableMetadata))
            {
                var columns = ColumnHelper.GetFildesInfo<TEntity>();
                TableHelper.GetTableNameAndSchema<TEntity>(out string tableName, out string schema);
             
                tableMetadata = new TableInfo (columns, tableName, schema);
                _metadataCache.TryAdd(nameOfT, tableMetadata);
            }

            return tableMetadata;
        }
    }
}

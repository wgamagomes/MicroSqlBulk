using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MicroSqlBulk.Helper
{
    public static class DataTableHelper
    {
        private static ConcurrentDictionary<string, SqlBulkEntityConfiguration> _mapperCache = new ConcurrentDictionary<string, SqlBulkEntityConfiguration>();

        public static DataTable ConvertToDatatable<TEntity>(List<TEntity> data)
        {
            var sqlBulkEntityConfiguration = GetSqlBulkEntityConfiguration(data);

            DataTable dataTable = new DataTable(sqlBulkEntityConfiguration.TableName);

            var dataColumns = sqlBulkEntityConfiguration.Columns
                                .Select(col => new DataColumn(col.Name, col.PropertyDescriptor.PropertyType))
                                .ToArray();

            dataTable.Columns.AddRange(dataColumns);

            foreach (var row in GetRows(data))
            {
                dataTable.Rows.Add(row.Values);
            }

            return dataTable;
        }
   
        static IEnumerable<Row> GetRows<TEntity>(List<TEntity> data)
        {
            var sqlBulkEntityConfiguration = GetSqlBulkEntityConfiguration<TEntity>(data);

            List<object> values = new List<object>();

            foreach (TEntity item in data)
            {
                foreach (var prop in sqlBulkEntityConfiguration.Columns.ToList())
                {
                    values.Add(prop.PropertyDescriptor.GetValue(item));
                }

                var row = new Row(values.ToArray());

                values.Clear();

                yield return row;
            }
        }

        static SqlBulkEntityConfiguration GetSqlBulkEntityConfiguration<TEntity>(List<TEntity> data)
        {
            SqlBulkEntityConfiguration sqlBulkEntityConfiguration;
            var nameOfT = typeof(TEntity).Name;

            if (!_mapperCache.TryGetValue(nameOfT, out sqlBulkEntityConfiguration))
            {
                var columns = ColumnHelper.GetFildesInfo<TEntity>();
                var tableName = TableHelper.GetTableName<TEntity>();
                sqlBulkEntityConfiguration = new SqlBulkEntityConfiguration(columns, tableName);
                _mapperCache.TryAdd(nameOfT, sqlBulkEntityConfiguration);
            }

            return sqlBulkEntityConfiguration;
        }
    }
}

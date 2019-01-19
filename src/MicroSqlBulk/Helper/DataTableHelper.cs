using MicroSqlBulk.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static IList<Column> GetColumns<TEntity>(this List<TEntity> data)
        {
            IList<Column> columns;

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(TEntity));

            columns = new List<Column>();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                if (AttributeHelper.TryGetCustomAttribute(prop, out ColumnAttribute columnAttribute))
                {
                    columns.Add(new Column((columnAttribute).Name, prop));
                }
                else
                {
                    if (!AttributeHelper.TryGetCustomAttribute(prop, out IgnoreAttribute ignoreAttribute))
                    {
                        throw new Exception($"The '{prop.Name}' property should be configured through the '{nameof(ColumnAttribute)}' or it should be ignored on the POCO through the '{nameof(IgnoreAttribute)}'.");
                    }
                }
            }

            return columns;
        }

 

        public static string GetTableName<TEntity>()
        {
            TableAttribute customAttribute = (TableAttribute)(typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault());

            if (customAttribute == null)
                throw new Exception($"The entity '{typeof(TEntity)}' should be configured through the '{nameof(TableAttribute)}'");

            var schema = !string.IsNullOrWhiteSpace(customAttribute.Schema) ? $"{customAttribute.Schema}." : string.Empty;
            var tableName = customAttribute.Name;

            return $"{schema}{tableName}";
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
                var columns = data.GetColumns();
                var tableName = GetTableName<TEntity>();
                sqlBulkEntityConfiguration = new SqlBulkEntityConfiguration(columns, tableName);
                _mapperCache.TryAdd(nameOfT, sqlBulkEntityConfiguration);
            }

            return sqlBulkEntityConfiguration;
        }
    }
}

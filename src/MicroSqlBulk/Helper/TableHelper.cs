using MicroSqlBulk.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroSqlBulk.Helper
{
    public static class TableHelper
    {
        public static Dictionary<Type, String> SqlDataTypes
        {
            get
            {
                Dictionary<Type, String> dataMapper = new Dictionary<Type, string>();
                dataMapper.Add(typeof(int), "BIGINT NOT NULL");
                dataMapper.Add(typeof(int?), "BIGINT");
                dataMapper.Add(typeof(long), "BIGINT NOT NULL");
                dataMapper.Add(typeof(long?), "BIGINT");
                dataMapper.Add(typeof(string), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(bool), "BIT NOT NULL");
                dataMapper.Add(typeof(bool?), "BIT");
                dataMapper.Add(typeof(DateTime), "DATETIME NOT NULL");
                dataMapper.Add(typeof(DateTime?), "DATETIME");
                dataMapper.Add(typeof(float), "FLOAT NOT NULL");
                dataMapper.Add(typeof(float?), "FLOAT");
                dataMapper.Add(typeof(decimal), "DECIMAL(18,0) NOT NULL");
                dataMapper.Add(typeof(decimal?), "DECIMAL(18,0)");
                dataMapper.Add(typeof(Guid), "UNIQUEIDENTIFIER NOT NULL");
                dataMapper.Add(typeof(Guid?), "UNIQUEIDENTIFIER");

                return dataMapper;
            }
        }

        public static void GetTableNameAndSchema<TEntity>(out string tableName, out string schema)
        {
            TableAttribute customAttribute = (TableAttribute)(typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault());

            if (customAttribute == null)
                throw new InvalidOperationException($"The '{typeof(TEntity)}' entity should be configured through the '{nameof(TableAttribute)}'");

            schema = !string.IsNullOrWhiteSpace(customAttribute.Schema) ? $"{customAttribute.Schema}" : string.Empty;
            tableName = customAttribute.Name;
        }       

        public static bool IsNullableEnum(this Type type)
        {
            Type u = Nullable.GetUnderlyingType(type);
            return (u != null) && u.IsEnum;
        }

        public static string GetSQLDataType(this Type type)
        {
            if (SqlDataTypes.TryGetValue(type, out string dataType))
            {
                return dataType;
            }

            if (IsNullableEnum(type))
                return "INT";

            if (type.IsEnum)
                return "INT NOT NULL";


            throw new KeyNotFoundException($"The type '{type.Name}' doesn't  match any key in the collection.");
        }

        public static string GetCreateTableScript<TEntity>(bool generateToTempTable = false)
        {
            var info = CacheHelper.GetTableInfo<TEntity>();

            var tableName = info.FullTableName;

            if (generateToTempTable)
                tableName = info.FullTempTableName;

            return info.Columns
                        .GetCreateTableScript(tableName);
        }

        public static string GetCreateTableScript(this IList<Column> columns, string tableName)
        {
            StringBuilder script = new StringBuilder();

            script.Append($"CREATE TABLE {tableName}");

            script.Append("(");

            script.Append(columns.ForEachColumn(column => $"{column.Name} {GetSQLDataType(column.PropertyDescriptor.PropertyType)}", false));

            script.Append(")");

            return script.ToString();
        }

        public static string FromSourceColumnsToTargetColumns<TEntity>()
        {
            var info = CacheHelper.GetTableInfo<TEntity>();

            return info.Columns
                       .FromSourceColumnsToTargetColumns(info.FullTempTableName, info.FullTableName);
        }

        public static string FromSourceColumnsToTargetColumns(this IList<Column> columns, string source, string target)
        {
            return columns
                   .ForEachColumn(col => $"{target}.{col.Name} = {source}.{col.Name}").ToString();

        }

        public static string GetOnClause<TEntity>()
        {
            var info = CacheHelper.GetTableInfo<TEntity>();

            Column primaryKeyColumn = info.Columns.FirstOrDefault(column => column.IsPrimaryKey);

            if (primaryKeyColumn == null)
                throw new MissingFieldException($"Unable to proceed with this operation, the primary key of the {info.TableName} table was not found.");

            return $"ON {info.FullTableName}.{primaryKeyColumn.Name} = {info.FullTempTableName}.{primaryKeyColumn.Name}";
        }

        public static string ConcatenateColumns<TEntity>()
        {
            var info = CacheHelper.GetTableInfo<TEntity>();
            var columns = info.Columns;

            return columns.ForEachColumn(col => col.Name).ToString();
        }

        public static string SetThePrefixInTheColumns(this IList<Column> columns, string alias)
        {
            return columns.ForEachColumn(col => $"{alias}.{col.Name}").ToString();
        }

        public static string SetThePrefixInTheColumns<TEntity>(bool fromTempTable = false)
        {
            var info = CacheHelper.GetTableInfo<TEntity>();
            var columns = info.Columns;
            var tableName = info.FullTableName;

            if (fromTempTable)
                tableName = info.FullTempTableName;

            return columns.SetThePrefixInTheColumns(tableName);
        }

        private static StringBuilder ForEachColumn(this IList<Column> columns, Func<Column, string> execute, bool ignorePrimaryKey = true)
        {
            StringBuilder script = new StringBuilder();

            for (int i = 0; i < columns.Count; i++)
            {
                Column column = columns[i];

                if (ignorePrimaryKey && column.IsPrimaryKey)
                    continue;

                script.Append(execute(column));

                if (i != columns.Count - 1)
                {
                    script.Append(",");
                }
            }

            return script;
        }
    }
}

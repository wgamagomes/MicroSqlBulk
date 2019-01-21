
using MicroSqlBulk.Attributes;
using System;
using System.Linq;

namespace MicroSqlBulk.Helper
{
    public static class TableHelper
    {
        public static string GetTableName<TEntity>()
        {
            TableAttribute customAttribute = (TableAttribute)(typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault());

            if (customAttribute == null)
                throw new InvalidOperationException($"The entity '{typeof(TEntity)}' should be configured through the '{nameof(TableAttribute)}'");

            var schema = !string.IsNullOrWhiteSpace(customAttribute.Schema) ? $"{customAttribute.Schema}." : string.Empty;
            var tableName = customAttribute.Name;

            return $"{schema}{tableName}";
        }

    }
}

using System.Collections.Generic;


namespace MicroSqlBulk
{
    public class SqlBulkEntityConfiguration
    {
        public IList<Column> Columns { get; }
        public string TableName { get; }

        public SqlBulkEntityConfiguration(IList<Column> columns, string tableName)
        {
            Columns = columns;
            TableName = tableName;
        }
    }
}

using MicroSqlBulk.Interfaces;
using System.Collections.Generic;


namespace MicroSqlBulk
{
    public class SqlBulkEntityConfiguration: IMicroSqlConfiguration
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

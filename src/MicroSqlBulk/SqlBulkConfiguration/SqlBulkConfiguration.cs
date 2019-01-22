using MicroSqlBulk.Interfaces;
using System.Collections.Generic;


namespace MicroSqlBulk
{
    public class SqlBulkConfiguration: IMicroSqlConfiguration
    {
        public IList<Column> Columns { get; }
        public string TableName { get; }

        public SqlBulkConfiguration(IList<Column> columns, string tableName)
        {
            Columns = columns;
            TableName = tableName;
        }
    }
}

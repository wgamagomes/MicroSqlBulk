using System.Collections.Generic;

namespace MicroSqlBulk.Interfaces
{
    public interface IMicroSqlConfiguration
    {
        IList<Column> Columns { get; }
        string TableName { get; }
    }
}
﻿using System.Collections.Generic;


namespace MicroSqlBulk
{
    public class TableInfo 
    {
        public IList<Column> Columns { get; }
        public string TableName { get; }
        public string SchemaName { get; }

        public TableInfo (IList<Column> columns, string tableName, string schemaName)
        {
            Columns = columns;
            TableName = tableName;
            SchemaName = schemaName;
        }

        public string FullTableName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SchemaName))
                    return $"{SchemaName}.{TableName}";

                return TableName;

            }
        }

        public string FullTempTableName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SchemaName))
                    return $"{SchemaName}.#{TableName}_TEMP";

                return $"#{TableName}_TEMP";
            }
        }
    }
}

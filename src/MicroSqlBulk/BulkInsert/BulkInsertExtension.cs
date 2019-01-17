using MicroSqlBulk.Helper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MicroSqlBulk
{
    public static class BulkInsertExtension
    {
        public static void BulkInsert<TEntity>(this IDbConnection dbConnection, List<TEntity> data, bool closeConnection = true)
        {
            var datatable = DataTableHelper.ConvertToDatatable(data);

            SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                        (SqlConnection)dbConnection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                    );

            bulkCopy.DestinationTableName = datatable.TableName;

            dbConnection.Open();

            bulkCopy.WriteToServer(datatable);

            if (closeConnection)
                dbConnection.Close();
        }
    }
}

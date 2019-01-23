using MicroSqlBulk.Helper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MicroSqlBulk
{
    public static partial class MicroSqlBulkExtension
    {
        public static void BulkInsert<TEntity>(this IDbConnection dbConnection, IEnumerable<TEntity> data, int timeout = 30, bool openConnection = true, bool closeConnection = true)
        {
            var datatable = DataTableHelper.ConvertToDatatable(data.ToList());

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
            bulkCopy.BulkCopyTimeout = timeout;

            if (openConnection)
                dbConnection.Open();

            bulkCopy.WriteToServer(datatable);

            if (closeConnection)
                dbConnection.Close();
        }
    }
}

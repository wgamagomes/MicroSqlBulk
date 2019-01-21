using MicroSqlBulk.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MicroSqlBulk
{
    public static partial class MicroSqlBulkExtension
    {
        public static void BulkUpdate<TEntity>(this IDbConnection dbConnection, IEnumerable<TEntity> data, int timeout = 30, bool openConnection = true, bool closeConnection = true)
        {
            DataTable dt = new DataTable("DataTable");
            dt = DataTableHelper.ConvertToDatatable(data.ToList());

            SqlConnection conn = (SqlConnection)dbConnection;

            using (SqlCommand command = new SqlCommand("", conn))
            {
                try
                {
                    if(openConnection)
                        conn.Open();
                    
                    command.CommandText = "CREATE TABLE #TmpTable(...)";
                    command.ExecuteNonQuery();
                    
                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                    {
                        bulkcopy.BulkCopyTimeout = 660;
                        bulkcopy.DestinationTableName = "TableNameHere";
                        bulkcopy.WriteToServer(dt);
                        bulkcopy.BulkCopyTimeout = timeout;
                        bulkcopy.Close();
                    }
                    
                    command.CommandTimeout = timeout;
                    command.CommandText = "UPDATE T SET ... FROM " + "TableNameHere" + " T INNER JOIN #TmpTable Temp ON ...; DROP TABLE #TmpTable;";
                    command.ExecuteNonQuery();
                }
                finally
                {
                    if(closeConnection)
                        conn.Close();
                }
            }
        }
    }
}

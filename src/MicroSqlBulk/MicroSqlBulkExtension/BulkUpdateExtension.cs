using System;
using System.Collections.Generic;
using System.Data;

namespace MicroSqlBulk
{
    public static partial class MicroSqlBulkExtension
    {
        public static void BulkUpdate<TEntity>(this IDbConnection dbConnection, IEnumerable<TEntity> data, int timeout = 30, bool openConnection = true, bool closeConnection = true)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest
{
    public class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            string datasource = "shift-test.database.windows.net";

            string database = "Dev";
            string username = "dev";
            string password = "R4zypgHmvz9ZE9Ek";

            return DBSQLServerUtils.GetDBConnection(datasource, database, username, password);
        }
    }
}

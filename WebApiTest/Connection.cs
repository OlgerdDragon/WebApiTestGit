using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest
{
    public class Connection
    {
        string connStr = "server=localhost;user=root;database=shopping;paword=root;";
        SqlConnection conn = DBUtils.GetDBConnection();
    }
}

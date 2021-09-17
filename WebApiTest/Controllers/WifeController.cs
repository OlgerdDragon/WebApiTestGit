using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Controllers
{
    public class WifeController : Controller
    {
        [HttpPost]
        [Route("CreatedProductList")]
        public string CreatedProductList() 
        {
            
            return "I done CreatedProductList"; 
        }
        [HttpPost]
        [Route("AddProduct")]
        public string AddProduct() { return "I done AddProduct"; }
        [HttpDelete]
        [Route("DeletedProduct")]
        public string GetNededProduct() { return "I done GetNededProduct"; }
        [HttpGet]
        [Route("GetBougthList")]
        public string GetBougthList() 
        {
            string test = "";
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            string getSQLBougthList = "SELECT TOP (1000) [ProductID],[ProductName],[ProductPrice] FROM[dbo].[Products]";
            SqlCommand commandBougthList = new SqlCommand(getSQLBougthList,conn);
            SqlDataReader reader = commandBougthList.ExecuteReader();
            while (reader.Read())
            {
                test += reader[1].ToString();
                test += " ";
            }

            return "I done GetBougthList" + test;
        }
        
    }
}

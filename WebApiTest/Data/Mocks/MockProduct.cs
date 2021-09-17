using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Data.Interface;
using WebApiTest.Models;

namespace WebApiTest.Data.Mocks
{
    public class MockProduct : IProductsService
    {
        public IEnumerable<Product> AllProducts(int shopID)
        {

            List<Product> productsList = new List<Product>();
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            string getSQLProductList = "SELECT [Products].[ProductID],[ProductName],[ProductPrice] FROM[dbo].[Products] INNER JOIN Availabil ON Availabil.ProductID = [Products].ProductID WHERE ShopID ="+shopID+";";
            
            SqlCommand commandBougthList = new SqlCommand(getSQLProductList, conn);
            SqlDataReader reader = commandBougthList.ExecuteReader();
            while (reader.Read())
            {
                productsList.Add(new Product { Id = Convert.ToInt32(reader[0].ToString()), Name = reader[1].ToString(), Price = Convert.ToInt32(reader[0].ToString()) });
            }
            conn.Close();
            return productsList;
        }
    }
}

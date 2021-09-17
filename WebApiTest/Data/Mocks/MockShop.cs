using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Data.Interface;
using WebApiTest.Models;

namespace WebApiTest.Data.Mocks
{
    public class MockShop : IShopsService
    {
        SqlConnection conn = DBUtils.GetDBConnection();
        
        public IEnumerable<Shop> AllShops
        {
            get 
            {
                List<Shop> shopsList = new List<Shop>();
                conn.Open();
                //string getSQLBougthList = "SELECT [ProductID],[ProductName],[ProductPrice] FROM[dbo].[Products]";
                string getSQLShopsList = "SELECT [ShopID],[ShopName] FROM[dbo].[Shops];";
                SqlCommand commandBougthList = new SqlCommand(getSQLShopsList, conn);
                SqlDataReader reader = commandBougthList.ExecuteReader();
                while (reader.Read())
                {
                    shopsList.Add(new Shop {Id= Convert.ToInt32(reader[0].ToString()), Name = reader[1].ToString() });
                }
                conn.Close();
                return shopsList;
            }
            
        }
        public void AddProductInShop(int shopID, int productID)
        {
            conn.Open();
            string getSQLAddProductInShop = "INSERT INTO Availabil(ShopID,ProductID) VALUES ('" + shopID + "', '" + productID + "');";
            SqlCommand commandAddProductInShop = new SqlCommand(getSQLAddProductInShop, conn);
            SqlDataReader reader = commandAddProductInShop.ExecuteReader();
            conn.Close();
        }
        public void AddProduct(int productID, string productName, int price)
        {
            conn.Open();
            string getSQLAddProduct = "INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('" + productID + "','" + productName + "', "+ price + ");";
            SqlCommand commandAddProduct = new SqlCommand(getSQLAddProduct, conn);
            SqlDataReader reader = commandAddProduct.ExecuteReader();
            conn.Close();
        }
        public void AddShop(int shopID, string shopName)
        {
            conn.Open();
            string getSQLAddShop = "INSERT INTO Shops (ShopID, ShopName) VALUES ('"+ shopID + "','" + shopName + "');";
            SqlCommand commandAddShop = new SqlCommand(getSQLAddShop, conn);
            SqlDataReader reader = commandAddShop.ExecuteReader();
            conn.Close();
        }

        public void RemoveShop(int shopID)
        {
            conn.Open();
            string getSQLRemoveShop =  "DELETE FROM[dbo].[Shops] WHERE ShopID = " + shopID + "; ";
            SqlCommand commandAddShop = new SqlCommand(getSQLRemoveShop, conn);
            SqlDataReader reader = commandAddShop.ExecuteReader();
            conn.Close();
        }
        public void RemoveProduct(int productID)
        {
            conn.Open();
            string getSQLRemoveProduct = "DELETE FROM[dbo].[Products] WHERE ProductID = " + productID + "; ";
            SqlCommand commandAddShop = new SqlCommand(getSQLRemoveProduct, conn);
            SqlDataReader reader = commandAddShop.ExecuteReader();
            conn.Close();
        }
        public void RemoveProductInShop(int shopID, int productID)
        {
            conn.Open();
            string getSQLRemoveProductInShop = "DELETE FROM[dbo].[Products] WHERE ShopID = " + shopID + " AND ProductID = " + productID + "; ";
            SqlCommand commandAddShop = new SqlCommand(getSQLRemoveProductInShop, conn);
            SqlDataReader reader = commandAddShop.ExecuteReader();
            conn.Close();
        }

    }
}

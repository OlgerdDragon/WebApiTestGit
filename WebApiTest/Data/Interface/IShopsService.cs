using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Data.Interface
{
    public interface IShopsService
    {
        IEnumerable<Shop> AllShops {  get; }
        public void AddProductInShop(int shopID, int productID);
        public void AddProduct(int productID, string productName, int price);
        public void AddShop(int shopID, string shopName);
        public void RemoveShop(int shopID);
        public void RemoveProduct(int productID);
        public void RemoveProductInShop(int shopID, int productID);



    }
}

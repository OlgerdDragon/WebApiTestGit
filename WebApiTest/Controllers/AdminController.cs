using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiTest.Data.Interface;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IShopsService _allShops;
        private readonly IProductsService _allProducts;
        public AdminController(IShopsService iAllShops, IProductsService iAllProducts)
        {
            _allShops = iAllShops;
            _allProducts = iAllProducts;
        }
        [HttpGet]
        public string Get()
        {
            return "Hello Admin!";
        }

        [HttpGet]
        [Route("GetShopsList")]
        public string GetShopsList()
        {
            string shopList = JsonSerializer.Serialize<IEnumerable<Shop>>(_allShops.AllShops);
            return shopList;
        }
        [HttpGet]
        [Route("GetProductList")]
        public string GetProductsList(int shopID)
        {
            string productList = JsonSerializer.Serialize<IEnumerable<Product>>(_allProducts.AllProducts(shopID));
            return productList;
        }
        [HttpGet]
        [Route("AddProductInBd")]
        public string AddProduct(int productID, string productName, int price)
        {
            _allShops.AddProduct(productID, productName, price);
            return "I add product in BD!    :)";
        }
        [HttpGet]
        [Route("AddProductInShop")]
        public string AddProductInShops(int shopID, int productID)
        {
            _allShops.AddProductInShop(shopID, productID);
            return "I add product in shop!    :)";
        }
        [HttpGet]
        [Route("AddShop")]
        public string AddShop(int shopID, string shopName)
        {
            _allShops.AddShop(shopID, shopName);
            return "I add shop!    :)";
        }

        [HttpGet]
        [Route("RemoveShop")]
        public string RemoveShop(int shopID)
        {
            _allShops.RemoveShop(shopID);
            return "I removed Shop!    ";
        }

        [HttpGet]
        [Route("RemoveProduct")]
        public string RemoveProduct(int shopID, int productID)
        {
            _allShops.RemoveProduct(productID);
            return "I removed Product!    ";
        }

        [HttpGet]
        [Route("RemoveProductInShop")]
        public string RemoveProductInShop(int shopID, int productID)
        {
            _allShops.RemoveProductInShop(shopID, productID);
            return "I removed Product in Shop!    ";
        }

    }
}

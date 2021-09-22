using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Data;
using WebApiTest.Models;

namespace WebApiTest.Services.HusbandService
{
    public class HusbandService : IHusbandService
    {

        private readonly TowerContext _context;

        public HusbandService(TowerContext context)
        {
            _context = context;
        }
        public async Task<List<WantedList>> GetWantedListAsync()
        {
            return await _context.WantedLists.Select(i => new WantedList
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToListAsync();
        }
        public async Task<List<Shop>> GetShopsForVisitAsync()
        {

            var neededProductList = GetWantedList();
            var productList = GetProductList();

            var neededShopList = new List<Shop>();
            foreach (var neededProduct in neededProductList)
            {
                var productSearched = await _context.Products.FindAsync(neededProduct.NameProduct);
                var shopSearched = await _context.Shops.FindAsync(productSearched.ShopId);
                if (!neededShopList.Contains(shopSearched))
                {
                    neededShopList.Add(shopSearched);
                }
            }

            return  neededShopList;
        }
        public async Task<List<Product>> GetProductsInShopAsync(int ShopId)
        {
            var neededProductList = GetWantedList();

            var productsList = new List<Product>();
            foreach (var neededProduct in neededProductList)
            {
                var productSearched = await _context.Products.FindAsync(neededProduct.NameProduct, ShopId);
            }

            return productsList;
        }
        public async Task<Shop> FindShopAsync(long id)
        {
            return await _context.Shops.FindAsync(id);
        }
        public async Task<Product> FindProductAsync(string name)
        {
            return await _context.Products.FindAsync(name);
        }
        List<WantedList> GetWantedList()
        {
            return _context.WantedLists.Select(i => new WantedList
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToList();
        }
        List<Product> GetProductList()
        {
            return _context.Products.Select(i => new Product
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                ShopId = i.ShopId
            }).ToList();
        }
    }
}

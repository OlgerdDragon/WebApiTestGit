using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.HusbandService
{
    public class HusbandService : IHusbandService
    {

        private readonly TowerContext _context;
       

        public HusbandService(TowerContext context)
        {
            _context = context;
        }
        public async Task<List<WantedListDto>> GetWantedListAsync()
        {
            return await _context.WantedLists.Select(i => new WantedListDto
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToListAsync();
        }
        public async Task<List<ShopDto>> GetShopsForVisitAsync()
        {
            
            var neededProductList = GetWantedList();
            var productList = GetProductList();
            
            var neededShopList = new List<ShopDto>();
            foreach (var neededProduct in neededProductList)
            {
                var shopSearched = SearchShop(productList, neededProduct);
                if (shopSearched == null) break;
                
                var shopSearchedDto = new ShopDto
                {
                    Id = shopSearched.Id,
                    Name = shopSearched.Name
                };

                if (!neededShopList.Contains(shopSearchedDto))
                {
                    neededShopList.Add(shopSearchedDto);
                }
            }

            return neededShopList;
        }
        Shop SearchShop(List<ProductDto> productList, WantedListDto neededProduct)
        {
            foreach (var product in productList)
            {
                if (product.Name == neededProduct.NameProduct)
                {
                    var productSearched = product;
                    return _context.Shops.FindAsync(product.ShopId).Result;
                }
            }
            return null;
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
        public async Task<Shop> FindShopAsync(int id)
        {
            return await _context.Shops.FindAsync(id);
        }
        public async Task<Product> FindProductAsync(string name)
        {
            return await _context.Products.FindAsync(name);
        }
        List<WantedListDto> GetWantedList()
        {
            return _context.WantedLists.Select(i => new WantedListDto
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToList();
        }
        List<ProductDto> GetProductList()
        {
            return _context.Products.Select(i => new ProductDto
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                ShopId = i.ShopId
            }).ToList();
        }
    }
}

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
        public async Task<List<WantedProductDto>> GetWantedProductsAsync()
        {
            return await _context.WantedProducts.Select(i => new WantedProductDto
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToListAsync();
        }
        public async Task<List<ShopDto>> GetShopsForVisitAsync()
        {
            
            var neededProductList = GetWantedProducts();
            var productList = GetProductList();
            
            var neededShopList = new List<ShopDto>();
            foreach (var neededProduct in neededProductList)
            {
                var shopSearched = SearchShop(productList, neededProduct);
                if (shopSearched != null)
                {
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
            }

            return neededShopList;
        }
        Shop SearchShop(List<ProductDto> productList, WantedProductDto neededProduct)
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
        
        public async Task<List<ProductDto>> GetProductsInShopAsync(int ShopId)
        {
            var neededProductList = GetWantedProducts();
            var productList = GetProductList();
            var productInShop = new List<ProductDto>();
            foreach (var product in productList)
            {
                if (product.ShopId == ShopId) 
                    
                    foreach (var neededProduct in neededProductList)
                    {
                        if (neededProduct.NameProduct==product.Name)
                        {
                            var newProduct = new ProductDto
                            {
                                Id = product.Id,
                                Name = product.Name,
                                Price = product.Price,
                                ShopId = product.ShopId
                            };
                            productInShop?.Add((newProduct));
                        }
                    }
            }
            return productInShop;
        }

       
        public async Task<Shop> FindShopAsync(int id)
        {
            return await _context.Shops.FindAsync(id);
        }
        public async Task<Product> FindProductAsync(string name)
        {
            return await _context.Products.FindAsync(name);
        }
        List<WantedProductDto> GetWantedProducts()
        {
            return _context.WantedProducts.Select(i => new WantedProductDto
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

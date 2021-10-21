using System;
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

        private readonly TownContext _context;
       

        public HusbandService(TownContext context)
        {
            _context = context;
        }
        public async Task<List<WantedProductDto>> GetWantedProductsAsync()
        {
            try
            {
                return await _context.WantedProducts.Select(i => new WantedProductDto
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId,
                    WifeId = i.WifeId
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("GetWantedProductsAsync method:" + ex.Message);
            }
            
        }
        public async Task<List<ShopDto>> GetShopsForVisitAsync()
        {
            try
            {
                var neededProductList = GetWantedProducts();
                var productList = GetProductList();

                var neededShopList = new List<ShopDto>();
                foreach (var neededProduct in neededProductList)
                {
                    var shopSearched = await SearchShop(productList, neededProduct);
                    if (shopSearched != null)
                    {
                        var shopSearchedDto = ShopDto.ItemShopDTO(shopSearched);

                        if (!neededShopList.Exists(o => o.Id == shopSearchedDto.Id))
                        {
                            neededShopList.Add(shopSearchedDto);
                        }
                    }
                }
                return neededShopList;
            }
            catch (Exception ex)
            {
                throw new Exception("GetShopsForVisitAsync method:" + ex.Message);
            }
        }
        async Task<Shop> SearchShop(List<ProductDto> productList, WantedProductDto neededProduct)
        {
            try
            {
                foreach (var product in productList)
                {
                    if (product.Id == neededProduct.ProductId)
                    {
                        var productSearched = product;
                        return await _context.Shops.FindAsync(product.ShopId);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SearchShop method:" + ex.Message);
            }
        }
        
        public async Task<List<ProductDto>> GetProductsInShopAsync(int ShopId)
        {
            try
            {
                var neededProductList = GetWantedProducts();
                var productList = GetProductList();
                var productInShop = new List<ProductDto>();
                foreach (var product in productList)
                {
                    if (product.ShopId == ShopId)

                        foreach (var neededProduct in neededProductList)
                        {
                            if (neededProduct.ProductId == product.Id)
                            {
                                productInShop?.Add(product);
                            }
                        }
                }
                return productInShop;
            }
            catch (Exception ex)
            {
                throw new Exception("GetProductsInShopAsync method:" + ex.Message);
            }
        }

       
        public async Task<Shop> FindShopAsync(int id)
        {
            try
            {
                return await _context.Shops.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("FindShopAsync method:" + ex.Message);
            }
        }
        public async Task<Product> FindProductAsync(string name)
        {
            try
            {
                return await _context.Products.FindAsync(name);
            }
            catch (Exception ex)
            {
                throw new Exception("FindProductAsync method:" + ex.Message);
            }
        }
        List<WantedProductDto> GetWantedProducts()
        {
            try
            {
                return _context.WantedProducts.Select(i => new WantedProductDto
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("GetWantedProducts method:" + ex.Message);
            }
        }
        List<ProductDto> GetProductList()
        {
            try
            {
                return _context.Products.Select(i => new ProductDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    ShopId = i.ShopId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("GetProductList method:" + ex.Message);
            }
        }
    }
}

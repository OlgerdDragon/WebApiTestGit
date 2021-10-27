using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.HusbandService
{
    public class HusbandService : IHusbandService
    {

        private readonly TownContext _context;
        private readonly ILogger<HusbandService> _logger;

        public HusbandService(TownContext context, ILogger<HusbandService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<List<WantedProductDto>>> GetWantedProductsAsync()
        {
            try
            {
                var wantedProducts = await _context.WantedProducts.Select(i => new WantedProductDto
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId,
                    WifeId = i.WifeId
                }).ToListAsync();
                _logger.LogInformation("GetWantedProductsAsync: ", wantedProducts);
                return new Result<List<WantedProductDto>>(wantedProducts);
            }
            catch (Exception ex)
            {
                throw new Exception("GetWantedProductsAsync method:" + ex.Message);
            }
            
        }
        public async Task<Result<List<ShopDto>>> GetShopsForVisitAsync()
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
                        var shopSearchedDto = ShopDto.ItemShopDTO(shopSearched.Element);

                        if (!neededShopList.Exists(o => o.Id == shopSearchedDto.Id))
                        {
                            neededShopList.Add(shopSearchedDto);
                        }
                    }
                }
                return new Result<List<ShopDto>>(neededShopList);
            }
            catch (Exception ex)
            {
                throw new Exception("GetShopsForVisitAsync method:" + ex.Message);
            }
        }
        async Task<Result<Shop>> SearchShop(List<ProductDto> productList, WantedProductDto neededProduct)
        {
            try
            {
                Shop shop = null;
                foreach (var product in productList)
                {
                    if (product.Id == neededProduct.ProductId)
                    {
                        var productSearched = product;
                        shop = await _context.Shops.FindAsync(product.ShopId);
                    }
                }
                return new Result<Shop>(shop);
            }
            catch (Exception ex)
            {
                throw new Exception("SearchShop method:" + ex.Message);
            }
        }
        
        public async Task<Result<List<ProductDto>>> GetProductsInShopAsync(int ShopId)
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
                return new Result<List<ProductDto>>(productInShop);
            }
            catch (Exception ex)
            {
                throw new Exception("GetProductsInShopAsync method:" + ex.Message);
            }
        }

       
        public async Task<Result<Shop>> FindShopAsync(int id)
        {
            try
            {
                var shop = await _context.Shops.FindAsync(id);
                return new Result<Shop>(shop);
            }
            catch (Exception ex)
            {
                throw new Exception("FindShopAsync method:" + ex.Message);
            }
        }
        public async Task<Result<Product>> FindProductAsync(string name)
        {
            try
            {
                var product = await _context.Products.FindAsync(name);
                return new Result<Product>(product);
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

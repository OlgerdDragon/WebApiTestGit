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
                return new Result<List<WantedProductDto>>(wantedProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetWantedProductsAsync");
                return new Result<List<WantedProductDto>>();
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
                            _logger.LogDebug($"GetShopsForVisitAsync - Add(shop) - shopSearchedDto.Id: {shopSearchedDto.Id} shopSearchedDto.Name: {shopSearchedDto.Name}");

                        }
                    }
                }
                return new Result<List<ShopDto>>(neededShopList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetShopsForVisitAsync");
                return new Result<List<ShopDto>>();
            }
        }
        async Task<Result<Shop>> SearchShop(List<ProductDto> productList, WantedProductDto neededProduct)
        {
            try
            {
                _logger.LogDebug($"SearchShop return - neededProduct.Id: {neededProduct.Id} neededProduct.ProductId: {neededProduct.ProductId}");
                
                Shop shop = null;
                foreach (var product in productList)
                {
                    if (product.Id == neededProduct.ProductId)
                    {
                        var productSearched = product;
                        shop = await _context.Shops.FindAsync(product.ShopId);
                    }
                }
                _logger.LogDebug($"SearchShop return - shop.Id: {shop.Id} shop.Name: {shop.Name}");
                return new Result<Shop>(shop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"SearchShop return - neededProduct.Id: {neededProduct.Id} neededProduct.ProductId: {neededProduct.ProductId}"); ;
                return new Result<Shop>();
            }
        }
        
        public async Task<Result<List<ProductDto>>> GetProductsInShopAsync(int ShopId)
        {
            try
            {
                _logger.LogDebug($"GetProductsInShopAsync - ShopId: {ShopId} ");
                var neededProductList = GetWantedProducts();
                var productList = GetProductList();
                var productInShop = new List<ProductDto>();

                foreach (var product in productList)
                {
                    if (product.ShopId == ShopId)
                    {
                        foreach (var neededProduct in neededProductList)
                        {
                            if (neededProduct.ProductId == product.Id)
                            {
                                productInShop?.Add(product);
                                _logger.LogDebug($"GetProductsInShopAsync - Add(product) - product.Id: {product.Id} product.Name: {product.Name} product.Price: {product.Price} product.ShopId: {product.ShopId}");
                            }
                        }
                    }
                }
                return new Result<List<ProductDto>>(productInShop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetProductsInShopAsync - ShopId: { ShopId}");
                return new Result<List<ProductDto>>();
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
                _logger.LogError(ex, $"FindShopAsync  id: {id}");
                return new Result<Shop>();
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
                _logger.LogError(ex, "GetWantedProducts");
                throw ex;
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
                _logger.LogError(ex, "GetProductList");
                throw ex; 
            }
        }
    }
}

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
        public async Task<Result<List<WantedProductDto>>> GetWantedProductsAsync(string userLogin)
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
                _logger.LogError(ex, $"GetWantedProductsAsync userLogin: {userLogin}");
                return new Result<List<WantedProductDto>>(ex);
            }
            
        }
        public async Task<Result<List<ShopDto>>> GetShopsForVisitAsync(string userLogin)
        {
            try
            {
                var neededProductList = GetWantedProducts();
                var neededShopList = new List<ShopDto>();
                foreach (var neededProduct in neededProductList)
                {
                    var productSearched = await FindProductAsync(neededProduct.ProductId);
                    var shopSearched = await FindShopAsync(productSearched.Element.Id);
                    var shopSearchedDto = ShopDto.ItemShopDTO(shopSearched.Element);
                    if (!neededShopList.Contains(shopSearchedDto))
                        neededShopList.Add(shopSearchedDto);
                    _logger.LogDebug($"GetShopsForVisitAsync userLogin: {userLogin} - List.Add(shop) - shopSearchedDto.Id: {shopSearchedDto.Id} shopSearchedDto.Name: {shopSearchedDto.Name}");
                }
                return new Result<List<ShopDto>>(neededShopList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetShopsForVisitAsync");
                return new Result<List<ShopDto>>(ex);
            }
        }
        
        public async Task<Result<List<ProductDto>>> GetProductsInShopAsync(int ShopId, string userLogin)
        {
            try
            {
                _logger.LogDebug($"GetProductsInShopAsync  userLogin: {userLogin} - ShopId: {ShopId} ");
                var neededProductList = GetWantedProducts();
                var productInShop = new List<ProductDto>();

                foreach (var neededProduct in neededProductList)
                {
                    _logger.LogDebug($"GetProductsInShopAsync - neededProduct.Id: {neededProduct.Id} neededProduct.BoughtStatus: {neededProduct.BoughtStatus} neededProduct.ProductId: {neededProduct.ProductId} neededProduct.WifeId: {neededProduct.WifeId}");
                    var productSearched = await FindProductAsync(neededProduct.ProductId);
                    if (productSearched.Element.ShopId == ShopId)
                    {
                        var productDto = ProductDto.ItemProductDTO(productSearched.Element);
                        productInShop.Add(productDto);
                        _logger.LogDebug($"GetProductsInShopAsync List.Add(product) - product.Id: {productDto.Id} product.Name: {productDto.Name} product.Price: {productDto.Price} product.ShopId: {productDto.ShopId}");
                    }
                    else _logger.LogDebug($"GetProductsInShopAsync List.Add(product) - Not added");
                }
                return new Result<List<ProductDto>>(productInShop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetProductsInShopAsync - ShopId: { ShopId}");
                return new Result<List<ProductDto>>(ex);
            }
        }

        async Task<Result<Shop>> FindShopAsync(int id)
        {
            try
            {
                var shop = await _context.Shops.FindAsync(id);
                if (shop == null)
                    _logger.LogDebug($"FindProductAsync return - id: {id}  Null oject");
                else
                    _logger.LogDebug($"FindProductAsync return - id: {id} product.Id: {shop.Id} product.Name: {shop.Name}");
                return new Result<Shop>(shop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"FindShopAsync  id: {id}");
                return new Result<Shop>(ex);
            }
        }
        async Task<Result<Product>> FindProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    _logger.LogDebug($"FindProductAsync return - id: {id}  Null oject");
                else
                    _logger.LogDebug($"FindProductAsync return - id: {id} product.Id: {product.Id} product.Name: {product.Name} product.Price: {product.Price} product.ShopId: {product.ShopId}");
                return new Result<Product>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"FindProductAsync id: {id}");
                return new Result<Product>(ex);
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
    }
}

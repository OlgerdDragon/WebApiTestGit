using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HusbandGrpcService.Data;
using HusbandGrpcService.Models.Dto;
using HusbandGrpcService.Models;
using Google.Protobuf.WellKnownTypes;

namespace HusbandGrpcService.Services
{
    public class HusbandGreeterService : HusbandGreeter.HusbandGreeterBase, IHusbandGreeterService
    {

        private readonly TownContext _context; 
        private readonly ILogger<HusbandGreeterService> _logger;
        public HusbandGreeterService(TownContext context, ILogger<HusbandGreeterService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public override Task<BoolValue> Health(Empty _, ServerCallContext context)
        {
            return Task.FromResult(new BoolValue() { Value = true });
        }
        public override async Task<GetWantedProductsReply> GetWantedProducts(UserLoginRequest request, ServerCallContext context)
        {
            try
            {
                var wantedProducts = await _context.WantedProducts.Select(i => new WantedProductDtoMessage
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId,
                    WifeId = i.WifeId
                }).ToListAsync();
                var result = new GetWantedProductsReply { Element = new ListOfWantedProductDto() };
                result.Element.WantedProductDtoMessage.AddRange(wantedProducts);
                result.Successfully = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetWantedProductsAsync userLogin: {request.UserLogin}");
                return new GetWantedProductsReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<GetShopsForVisitReply> GetShopsForVisit(UserLoginRequest request, ServerCallContext context)
        {
            try
            {
                var neededProductList = GetWantedProducts().Element;
                var neededShopList = new List<ShopDtoMessage>();
                foreach (var neededProduct in neededProductList)
                {
                    var productSearched = await FindProductAsync(neededProduct.ProductId);
                    var shopSearched = await FindShopAsync(productSearched.Element.ShopId);
                    var shopSearchedDto = ShopDto.ItemShopDTOMessage(shopSearched.Element);
                    if(!neededShopList.Contains(shopSearchedDto))
                    neededShopList.Add(shopSearchedDto);
                    _logger.LogDebug($"GetShopsForVisitAsync userLogin: {request.UserLogin} - List.Add(shop) - shopSearchedDto.Id: {shopSearchedDto.Id} shopSearchedDto.Name: {shopSearchedDto.Name}");
                }
                var result = new GetShopsForVisitReply { Element = new ListOfShopDto() };
                result.Element.ShopDtoMessage.AddRange(neededShopList);
                result.Successfully = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetShopsForVisitAsync");
                return new GetShopsForVisitReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<GetProductsInShopReply> GetProductsInShop(GetProductsInShopRequest request, ServerCallContext context)
        {
            try
            {
                _logger.LogDebug($"GetProductsInShopAsync  userLogin: {request.UserLogin} - ShopId: {request.ShopId} ");
                var neededProductList = GetWantedProducts().Element;
                var productInShop = new List<ProductDtoMessage>();

                foreach (var neededProduct in neededProductList)
                {
                    _logger.LogDebug($"GetProductsInShopAsync - neededProduct.Id: {neededProduct.Id} neededProduct.BoughtStatus: {neededProduct.BoughtStatus} neededProduct.ProductId: {neededProduct.ProductId} neededProduct.WifeId: {neededProduct.WifeId}");
                    var productSearched = await FindProductAsync(neededProduct.ProductId);
                    if (productSearched.Element.ShopId == request.ShopId)
                    {
                        var productDto = ProductDto.ItemProductDTOMessage(productSearched.Element);
                        productInShop.Add(productDto);
                        _logger.LogDebug($"GetProductsInShopAsync List.Add(product) - product.Id: {productDto.Id} product.Name: {productDto.Name} product.Price: {productDto.Price} product.ShopId: {productDto.ShopId}");
                    }
                    else _logger.LogDebug($"GetProductsInShopAsync List.Add(product) - Not added");
                }
                var result = new GetProductsInShopReply { Element = new ListOfProductDto() };
                result.Element.ProductDtoMessage.AddRange(productInShop);
                result.Successfully = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetProductsInShopAsync userLogin: {request.UserLogin}  - ShopId: { request.ShopId}");
                return new GetProductsInShopReply { ErrorMessage = ex.Message, Successfully = false };
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
        Result<List<WantedProductDto>> GetWantedProducts()
        {
            try
            {
                var wantedProductList = _context.WantedProducts.Select(i => new WantedProductDto
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId

                }).ToList();
                _logger.LogDebug($"GetWantedProducts return - wantedProductList: {wantedProductList}");
                return new Result<List<WantedProductDto>>(wantedProductList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetWantedProducts");
                return new Result<List<WantedProductDto>>(ex);
            }
        }


    }
}

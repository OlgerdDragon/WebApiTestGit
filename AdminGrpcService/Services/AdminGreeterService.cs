using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdminGrpcService.Data;
using AdminGrpcService.Models.Dto;
using AdminGrpcService.Models;

namespace AdminGrpcService.Services
{
    public class AdminGreeterService : AdminGreeter.AdminGreeterBase, IAdminGreeterService
    {

        private readonly TownContext _context;
        private readonly ILogger<AdminGreeterService> _logger;
        public AdminGreeterService(TownContext context, ILogger<AdminGreeterService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<GetProductsReply> GetProducts(UserLoginRequest request, ServerCallContext context)
        {
            try
            {
                var products = await _context.Products.Select(i => new ProductDtoMessage
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    ShopId = i.ShopId
                }).ToListAsync();
                var result = new GetProductsReply { Element = new ListOfProductDto() };
                result.Element.ProductDtoMessage.AddRange(products);
                result.Successfully = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetProductsAsync - UserLogin: {request.UserLogin}");
                return new GetProductsReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }

        public override async Task<GetShopsReply> GetShops(UserLoginRequest request, ServerCallContext context)
        {
            try
            {
                var shops = await _context.Shops.Select(i => new ShopDtoMessage
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToListAsync();
                var result = new GetShopsReply { Element = new ListOfShopDto() };
                result.Element.ShopDtoMessage.AddRange(shops);
                result.Successfully = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetShopsAsync - UserLogin: {request.UserLogin}");
                return new GetShopsReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<ShopReply> UpdateShop(ShopRequest request, ServerCallContext context)
        {
            try
            {
                var newShopDto = request.ShopDtoMessage;
                var shop = _context.Shops
                .Where(i => i.Id == newShopDto.Id)
                .FirstOrDefault();
                if (shop == null)
                    return new ShopReply { Element = new ShopDtoMessage() };
                _logger.LogInformation($"UpdateShopAsync userLogin: {request.UserLogin}  - newShop.Id: {newShopDto.Id} newShop.Name: {newShopDto.Name}");
                shop.Name = newShopDto.Name;
                var shopDtoMessage = ShopDto.ItemShopDTOMessage(shop);
                await SaveChangesAsync();
                var result = new ShopReply { Element = shopDtoMessage, Successfully = true };
                return result;
            }
            catch (Exception ex)
            {
                var newShopDto = request.ShopDtoMessage;
                _logger.LogError(ex, $"UpdateShopAsync userLogin: {request.UserLogin} - newShop.Id: {newShopDto.Id} newShop.Name: {newShopDto.Name}");
                return new ShopReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<ProductReply> UpdateProduct(ProductRequest request, ServerCallContext context)
        {
            try
            {
                var newProduct = request.ProductDtoMessage;
                var product = _context.Products
                .Where(i => i.Id == newProduct.Id)
                .FirstOrDefault();
                if (product == null)
                    return new ProductReply { Element = new ProductDtoMessage() };
                _logger.LogInformation($"UpdateProductAsync userLogin: {request.UserLogin} - newProduct.Id: {newProduct.Id} product.Name: {newProduct.Name} product.Price: {newProduct.Price} product.ShopId: {newProduct.ShopId}");
                product.Name = newProduct.Name;
                product.Price = newProduct.Price;
                product.ShopId = newProduct.ShopId;

                var productDtoMessage = ProductDto.ItemProductDTOMessage(product);
                await SaveChangesAsync();
                var result = new ProductReply { Element = productDtoMessage, Successfully = true };
                return result;
            }
            catch (Exception ex)
            {
                var newProduct = request.ProductDtoMessage;
                _logger.LogError(ex, $"UpdateProductAsync userLogin: {request.UserLogin} - newProduct.Id: {newProduct.Id} product.Name: {newProduct.Name} product.Price: {newProduct.Price} product.ShopId: {newProduct.ShopId}");
                return new ProductReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }

        public override async Task<ShopReply> GetShop(ItemRequest request, ServerCallContext context)
        {
            try
            {
                var id = request.Id;
                _logger.LogDebug($"GetShopAsync - id: {id} return FindShopAsync(id)");
                var shopItem = await FindShopAsync(id);
                if (shopItem.Element == null)
                {
                    if (shopItem.ExceptionMessage != null)
                    {
                        _logger.LogError(shopItem.ExceptionMessage, $"GetShopAsync id: {id}");
                        return new ShopReply { ErrorMessage = shopItem.ExceptionMessage.Message, Successfully = false };
                    }
                    return null;
                }
                var shopDtoMessage = ShopDto.ItemShopDTOMessage(shopItem.Element);
                var result = new ShopReply { Element = shopDtoMessage, Successfully = true };
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetShop id: {request.Id}");
                return new ShopReply { ErrorMessage = ex.Message, Successfully = false };
            }

        }
        public override async Task<ProductReply> GetProduct(ItemRequest request, ServerCallContext context)
        {
            try
            {
                var id = request.Id;
                _logger.LogDebug($"GetProductAsync - id: {id} return FindProductAsync(id)");
                var productItem = await FindProductAsync(id);
                if (productItem.Element == null)
                {
                    if (productItem.ExceptionMessage != null)
                    {
                        _logger.LogError(productItem.ExceptionMessage, $"GetProductAsync id: {id}");
                        return new ProductReply { ErrorMessage = productItem.ExceptionMessage.Message, Successfully = false };
                    }
                    return null;
                }

                var productDtoMessage = ProductDto.ItemProductDTOMessage(productItem.Element);
                var result = new ProductReply { Element = productDtoMessage, Successfully = true };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetProductAsync id: {request.Id}");
                return new ProductReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<BoolReply> RemoveProduct(ItemRequest request, ServerCallContext context)
        {
            try
            {
                var id = request.Id;
                var status = false;
                var productItem = await FindProductAsync(id);
                if (productItem.ExceptionMessage != null)
                {
                    _logger.LogError(productItem.ExceptionMessage, "RemoveShop");
                    return new BoolReply { ErrorMessage = productItem.ExceptionMessage.Message, Successfully = false };
                }

                if (productItem.Element != null)
                {
                    status = true;
                    _context.Products.Remove(productItem.Element);
                    await SaveChangesAsync();
                    _logger.LogInformation($"RemoveProduct - id: {id} userLogin: {request.UserLogin} return status: {status}");
                }

                var result = new BoolReply { Element = status, Successfully = true };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RemoveProduct id: {request.Id}");
                return new BoolReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }

        public override async Task<BoolReply> RemoveShop(ItemRequest request, ServerCallContext context)
        {
            try
            {
                var id = request.Id;
                var status = false;
                var shopItem = await FindShopAsync(id);
                if (shopItem.ExceptionMessage != null)
                {
                    _logger.LogError(shopItem.ExceptionMessage, "RemoveShop");
                    return new BoolReply { ErrorMessage = shopItem.ExceptionMessage.Message, Successfully = false };
                }

                if (shopItem.Element != null)
                {
                    status = true;
                    _context.Shops.Remove(shopItem.Element);
                    await SaveChangesAsync();
                    _logger.LogInformation($"RemoveShop - id: {id} userLogin: {request.UserLogin} return status: {status}");
                }

                var result = new BoolReply { Element = status, Successfully = true };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RemoveShop id: {request.Id}");
                return new BoolReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<BoolReply> AddProduct(ProductRequest request, ServerCallContext context)
        {
            try
            {
                var status = false;
                var productDtoItem = request.ProductDtoMessage;
                if (productDtoItem.Name != null)
                {
                    _logger.LogDebug($"AddProduct - productDtoItem.Name: {productDtoItem.Name} productDtoItem.Price: {productDtoItem.Price} productDtoItem.ShopId: {productDtoItem.ShopId}");
                    var shop = await FindShopAsync(productDtoItem.ShopId);
                    if (shop.ExceptionMessage != null)
                    {
                        _logger.LogError(shop.ExceptionMessage, "AddProduct");
                        return new BoolReply { ErrorMessage = shop.ExceptionMessage.Message, Successfully = false };
                    }
                    if (shop.Element != null)
                    {
                        var productItem = new Product
                        {
                            Id = productDtoItem.Id,
                            Name = productDtoItem.Name,
                            Price = productDtoItem.Price,
                            ShopId = productDtoItem.ShopId,
                            Shops = shop.Element
                        };
                        _context.Products.Add(productItem);
                        await SaveChangesAsync();
                        status = true;
                        _logger.LogInformation($"AddProduct userLogin: {request.UserLogin} - status: {status} productDtoItem.Name: {productItem.Name} productDtoItem.Price: {productItem.Price} productDtoItem.ShopId: {productItem.ShopId}");
                    }
                }
                var result = new BoolReply { Element = status, Successfully = true };
                return result;
            }
            catch (Exception ex)
            {
                var productDtoItem = request.ProductDtoMessage;
                _logger.LogError(ex, $"AddProduct userLogin: {request.UserLogin} - productDtoItem.Name: {productDtoItem.Name} productDtoItem.Price: {productDtoItem.Price} productDtoItem.ShopId: {productDtoItem.ShopId}");
                return new BoolReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<BoolReply> AddShop(ShopRequest request, ServerCallContext context)
        {
            try
            {
                var status = false;
                var shopDtoMessage = request.ShopDtoMessage;
                if (shopDtoMessage.Id == 0)
                    return new BoolReply { Element = status, Successfully = true };
                var shopDto = new Shop
                {
                    Id = shopDtoMessage.Id,
                    Name = shopDtoMessage.Name
                };
                _context.Shops.Add(shopDto);
                await SaveChangesAsync();

                _logger.LogInformation($"AddProduct userLogin: {request.UserLogin} - shop.Id: {shopDtoMessage.Id} shop.Name: {shopDtoMessage.Name}");
                status = true;
                var result = new BoolReply { Element = status, Successfully = true };
                return result;
            }
            catch (Exception ex)
            {
                var shopDtoMessage = request.ShopDtoMessage;
                _logger.LogError(ex, $"AddShop userLogin: {request.UserLogin} - shopDtoMessage.Id: {shopDtoMessage.Id} shopDtoMessage.Name: {shopDtoMessage.Name}");
                return new BoolReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        async Task<Result<int>> SaveChangesAsync()
        {
            try
            {
                var res = await _context.SaveChangesAsync();

                _logger.LogDebug($"SaveChangesAsync done!");
                return new Result<int>(res);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveChangesAsync");
                return new Result<int>(ex);
            }
        }
        async Task<Result<Shop>> FindShopAsync(int id)
        {
            try
            {
                var shop = await _context.Shops.FindAsync(id);
                if (shop == null)
                    _logger.LogDebug($"FindShopAsync return - id: {id}  Null oject");
                else
                    _logger.LogDebug($"FindShopAsync return - id: {id} shop.Id: {shop.Id} shop.Name: {shop.Name}");
                return new Result<Shop>(shop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"FindShopAsync id: {id}");
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
    }
}

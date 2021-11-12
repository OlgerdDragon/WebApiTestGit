using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;


namespace WebApiTest.Services.AdminService
{
    public class AdminService: IAdminService
    {
        private readonly TownContext _context;
        private readonly ILogger<AdminService> _logger;
        public AdminService(TownContext context, ILogger<AdminService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<List<ProductDto>>> GetProductsAsync()
        {
            try
            {
                return new Result<List<ProductDto>>(await _context.Products.Select(i => new ProductDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    ShopId = i.ShopId
                }).ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProductsAsync");
                return new Result<List<ProductDto>>(ex);
            }
        }

        public async Task<Result<List<ShopDto>>> GetShopsAsync()
        {
            try
            {
                var element = new Result<List<ShopDto>>(await _context.Shops.Select(i => new ShopDto
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToListAsync());
                return element;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetShopsAsync");
                return new Result<List<ShopDto>>(ex);
            }
        }
        public async Task<Result<ShopDto>> UpdateShopAsync(ShopDto newShopDto, string userLogin)
        {
            try
            {
                var shop = _context.Shops
                .Where(i => i.Id == newShopDto.Id)
                .FirstOrDefault();
                if(shop == null) 
                    return new Result<ShopDto>(new ShopDto());
                _logger.LogInformation($"UpdateShopAsync userLogin: {userLogin}  - newShop.Id: {newShopDto.Id} newShop.Name: {newShopDto.Name}");
                shop.Name = newShopDto.Name;
                var shopDto = ShopDto.ItemShopDTO(shop);
                await SaveChangesAsync();
                return new Result<ShopDto>(shopDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateShopAsync userLogin: {userLogin} - newShop.Id: {newShopDto.Id} newShop.Name: {newShopDto.Name}");
                return new Result<ShopDto>(ex);
            }
        }
        public async Task<Result<ProductDto>> UpdateProductAsync(ProductDto newProduct, string userLogin)
        {
            try
            {
                
                var product = _context.Products
                .Where(i => i.Id == newProduct.Id)
                .FirstOrDefault();
                if (product == null)
                    return new Result<ProductDto>(new ProductDto());
                _logger.LogInformation($"UpdateProductAsync userLogin: {userLogin} - newProduct.Id: {newProduct.Id} product.Name: {newProduct.Name} product.Price: {newProduct.Price} product.ShopId: {newProduct.ShopId}");
                product.Name = newProduct.Name;
                product.Price = newProduct.Price;
                product.ShopId = newProduct.ShopId;
                
                var productDto = ProductDto.ItemProductDTO(product);
                await SaveChangesAsync();
                return new Result<ProductDto>(productDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateProductAsync userLogin: {userLogin} - newProduct.Id: {newProduct.Id} product.Name: {newProduct.Name} product.Price: {newProduct.Price} product.ShopId: {newProduct.ShopId}");
                return new Result<ProductDto>(ex);
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
        public async Task<Result<ActionResult<ShopDto>>> GetShopAsync(int id)
        {
            try
            {
                _logger.LogDebug($"GetShopAsync - id: {id} return FindShopAsync(id)");
                var shopItem = await FindShopAsync(id);
                if (shopItem.Element == null)
                {
                    if(shopItem.ExceptionMessage != null)
                    {
                        _logger.LogError(shopItem.ExceptionMessage, $"GetShopAsync id: {id}");
                        return new Result<ActionResult<ShopDto>>(shopItem.ExceptionMessage);
                    }
                    return null;
                }
                return new Result<ActionResult<ShopDto>>(ShopDto.ItemShopDTO(shopItem.Element));
            }
            catch (Exception ex)
            {
                
                return new Result<ActionResult<ShopDto>>(ex);
            }
            
        }
        public async Task<Result<ActionResult<ProductDto>>> GetProductAsync(int id)
        {
            try
            {
                _logger.LogDebug($"GetProductAsync - id: {id} return FindProductAsync(id)");
                var productItem = await FindProductAsync(id);
                if (productItem.Element == null)
                {
                    if (productItem.ExceptionMessage != null)
                    {
                        _logger.LogError(productItem.ExceptionMessage, $"GetProductAsync id: {id}");
                        return new Result<ActionResult<ProductDto>>(productItem.ExceptionMessage);
                    }
                    return null;
                }

                return new Result<ActionResult<ProductDto>>(ProductDto.ItemProductDTO(productItem.Element));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetProductAsync id: {id}");
                return new Result<ActionResult<ProductDto>>(ex);
            }
        }
        public async Task<Result<bool>> RemoveProduct(int id, string userLogin)
        {
            try
            {
                var status = true;
                var productItem = await FindProductAsync(id);
                if (productItem.Element == null)
                    status = false;

                _context.Products.Remove(productItem.Element);
                await SaveChangesAsync();

                _logger.LogInformation($"RemoveProduct - id: {id} userLogin: {userLogin} return status: {status}");
                return new Result<bool>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RemoveProduct id: {id}");
                return new Result<bool>(ex);
            }
        }
        
        public async Task<Result<bool>> RemoveShop(int id, string userLogin)
        {
            try
            {
                var status = false;
                var shopItem = await FindShopAsync(id);
                if (shopItem.Element != null)
                {
                    status = true;
                    _context.Shops.Remove(shopItem.Element);
                    await SaveChangesAsync();
                    _logger.LogInformation($"RemoveShop - id: {id} userLogin: {userLogin} return status: {status}");
                }
                if (shopItem.ExceptionMessage != null)
                {
                    _logger.LogError(shopItem.ExceptionMessage, "RemoveShop");
                    return new Result<bool>(shopItem.ExceptionMessage);
                }
                _logger.LogDebug($"AddProduct - id: {id} userLogin: {userLogin} return status: {status}");
                return new Result<bool>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token");
                return new Result<bool>(ex);
            }
        }
        public async Task<Result<bool>> AddProduct(ProductDto productDtoItem, string userLogin)
        {
            try
            {
                if (productDtoItem.Name == null)
                    return new Result<bool>(false);
                _logger.LogDebug($"AddProduct - productDtoItem.Name: {productDtoItem.Name} productDtoItem.Price: {productDtoItem.Price} productDtoItem.ShopId: {productDtoItem.ShopId}");
                var shop = await FindShopAsync(productDtoItem.ShopId);
                if (shop.Element == null)
                {
                    if(shop.ExceptionMessage != null)
                    {
                        _logger.LogError(shop.ExceptionMessage, "AddProduct");
                        return new Result<bool>(shop.ExceptionMessage);
                    }
                    return new Result<bool>(false);
                }
                var productItem = productDtoItem.Product(shop.Element);

                _context.Products.Add(productItem);
                await SaveChangesAsync();

                _logger.LogInformation($"AddProduct userLogin: {userLogin} - productDtoItem.Name: {productItem.Name} productDtoItem.Price: {productItem.Price} productDtoItem.ShopId: {productItem.ShopId}");
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token"); 
                return new Result<bool>(ex);
            }
        }
        public async Task<Result<bool>> AddShop(ShopDto shopDtoItem, string userLogin)
        {
            try
            {
                if (shopDtoItem.Name == null)
                    return new Result<bool>(false);
                _context.Shops.Add(shopDtoItem.Shop());
                await SaveChangesAsync();

                _logger.LogInformation($"AddProduct userLogin: {userLogin} - shop.Id: {shopDtoItem.Id} shop.Name: {shopDtoItem.Name}");
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token");
                return new Result<bool>(ex);
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
    }
}

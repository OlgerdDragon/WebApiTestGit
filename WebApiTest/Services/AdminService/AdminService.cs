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
        public async Task<Result<ShopDto>> UpdateShopAsync(ShopDto newShop, string userLogin)
        {
            try
            {
                _logger.LogInformation($"UpdateShopAsync - newShop.Id: {newShop.Id} newShop.Name: {newShop.Name}");
                var shop = _context.Shops
                .Where(i => i.Id == newShop.Id)
                .FirstOrDefault();

                shop.Name = newShop.Name;
                await SaveChangesAsync();
                return new Result<ShopDto>(newShop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateShopAsync - newShop.Id: {newShop.Id} newShop.Name: {newShop.Name}");
                return new Result<ShopDto>(ex);
            }
        }
        public async Task<Result<ProductDto>> UpdateProductAsync(ProductDto newProduct, string userLogin)
        {
            try
            {
                _logger.LogInformation($"UpdateProductAsync - newProduct.Id: {newProduct.Id} product.Name: {newProduct.Name} product.Price: {newProduct.Price} product.ShopId: {newProduct.ShopId}");
                var product = _context.Products
                .Where(i => i.Id == newProduct.Id)
                .FirstOrDefault();

                product.Name = newProduct.Name;
                product.Price = newProduct.Price;
                product.ShopId = newProduct.ShopId;

                await SaveChangesAsync();
                return new Result<ProductDto>(newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateProductAsync - newProduct.Id: {newProduct.Id} product.Name: {newProduct.Name} product.Price: {newProduct.Price} product.ShopId: {newProduct.ShopId}");
                return new Result<ProductDto>(ex);
            }
        }
        public async Task<Result<Shop>> FindShopAsync(int id)
        {
            try
            {
                var shop = await _context.Shops.FindAsync(id);
                _logger.LogDebug($"FindShopAsync return - id: {id} shop.Id: {shop.Id} shop.Name: {shop.Name}");
                return new Result<Shop>(shop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"FindShopAsync id: {id}");
                return new Result<Shop>(ex);
            }
        }

        public async Task<Result<Product>> FindProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
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
                    return null;
                }
                return new Result<ActionResult<ShopDto>>(ShopDto.ItemShopDTO(shopItem.Element));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetShopAsync id: {id}");
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
                if (productItem == null)
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
                var status = true;
                var shopItem = await FindShopAsync(id);
                if (shopItem == null)
                    status = false;

                _context.Shops.Remove(shopItem.Element);
                await SaveChangesAsync();

                _logger.LogInformation($"RemoveShop - id: {id} userLogin: {userLogin} return status: {status}");
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
                _logger.LogDebug($"AddProduct - productDtoItem.Name: {productDtoItem.Name} productDtoItem.Price: {productDtoItem.Price} productDtoItem.ShopId: {productDtoItem.ShopId}");
                var shop = FindShopAsync(productDtoItem.ShopId);
                var productItem = productDtoItem.Product(shop.Result.Element);

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

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
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = LogEventLevel.Verbose;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.ControlledBy(levelSwitch)
                .CreateLogger();

            _context = context;
            _logger = logger;
        }
        public async Task<List<ProductDto>> GetProductsAsync()
        {
            try
            {
                return await _context.Products.Select(i => new ProductDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    ShopId = i.ShopId
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("GetProductsAsync method:" + ex.Message);
            }
        }

        public async Task<List<ShopDto>> GetShopsAsync()
        {
            try
            {
                return await _context.Shops.Select(i => new ShopDto
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("GetShopsAsync method:" + ex.Message);
            }
        }
        public async Task<ShopDto> UpdateShopAsync(ShopDto newShop)
        {
            try
            {
                var shop = _context.Shops
                .Where(i => i.Id == newShop.Id)
                .FirstOrDefault();

                shop.Name = newShop.Name;
                await SaveChangesAsync();
                return newShop;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateShopAsync method:" + ex.Message);
            }
        }
        public async Task<ProductDto> UpdateProductAsync(ProductDto newProduct)
        {
            try
            {
                var product = _context.Products
                .Where(i => i.Id == newProduct.Id)
                .FirstOrDefault();

                product.Name = newProduct.Name;
                product.Price = newProduct.Price;
                product.ShopId = newProduct.ShopId;

                await SaveChangesAsync();
                return newProduct;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateProductAsync method:" + ex.Message);
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

        public async Task<Product> FindProductAsync(int id)
        {
            try
            {
                return await _context.Products.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("FindProductAsync method:" + ex.Message);
            }
        }
        public async Task<ActionResult<ShopDto>> GetShopAsync(int id)
        {
            try
            {
                var shopItem = await FindShopAsync(id);
                if (shopItem == null)
                {
                    return null;
                }
                return ShopDto.ItemShopDTO(shopItem);
            }
            catch (Exception ex)
            {
                throw new Exception("GetShopAsync method:" + ex.Message);
            }
            
        }
        public async Task<ActionResult<ProductDto>> GetProductAsync(int id)
        {
            try
            {
                var productItem = await FindProductAsync(id);

                if (productItem == null)
                {
                    return null;
                }

                return ProductDto.ItemProductDTO(productItem);
            }
            catch (Exception ex)
            {
                throw new Exception("GetProductAsync method:" + ex.Message);
            }
        }
        public async Task<bool> RemoveProduct(int id)
        {
            try
            {
                var productItem = await FindProductAsync(id);
                if (productItem == null)
                {
                    return false;
                }
                _context.Products.Remove(productItem);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("RemoveProduct method:" + ex.Message);
            }
        }

        public async Task<bool> RemoveShop(int id)
        {
            try
            {
                var shopItem = await FindShopAsync(id);
                if (shopItem == null)
                {
                    return false;
                }
                _context.Shops.Remove(shopItem);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("RemoveShop method:" + ex.Message);
            }
        }
        public async Task AddProduct(ProductDto productDtoItem)
        {
            try
            {
                var shop = FindShopAsync(productDtoItem.ShopId);
                var productItem = productDtoItem.Product(shop.Result);

                _context.Products.Add(productItem);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("AddProduct method:" + ex.Message);
            }
        }
        public async Task AddShop(ShopDto shopDtoItem)
        {
            try
            {
                var shop = _context.Shops.Add(shopDtoItem.Shop());
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("AddShop method:" + ex.Message);
            }
        }
        async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("SaveChangesAsync method:" + ex.Message);
            }
        }
    }
}

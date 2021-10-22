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
            return await _context.Products.Select(i => new ProductDto
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                ShopId = i.ShopId
            }).ToListAsync();
        }

        public async Task<List<ShopDto>> GetShopsAsync()
        {
            return await _context.Shops.Select(i => new ShopDto
            {
                Id = i.Id,
                Name = i.Name
            }).ToListAsync();
        }
        public async Task<ShopDto> UpdateShopAsync(ShopDto newShop)
        {
            var shop = _context.Shops
                .Where(i => i.Id == newShop.Id)
                .FirstOrDefault();

            shop.Name = newShop.Name;
            await SaveChangesAsync();
            return newShop;
        }
        public async Task<ProductDto> UpdateProductAsync(ProductDto newProduct)
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
        public async Task<Shop> FindShopAsync(int id)
        {
            return await _context.Shops.FindAsync(id);
        }
        public async Task<Product> FindProductAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<ActionResult<ShopDto>> GetShopAsync(int id)
        {
            var shopItem = await FindShopAsync(id);
            if (shopItem == null)
            {
                return null;
            }
            return ShopDto.ItemShopDTO(shopItem);
        }
        public async Task<ActionResult<ProductDto>> GetProductAsync(int id)
        {
            var productItem = await FindProductAsync(id);

            if (productItem == null)
            {
                return null;
            }

            return ProductDto.ItemProductDTO(productItem);
        }
        public async Task<bool> RemoveProduct(int id)
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

        public async Task<bool> RemoveShop(int id)
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
        public async Task AddProduct(ProductDto productDtoItem)
        {
            var shop = FindShopAsync(productDtoItem.ShopId);
            var productItem = productDtoItem.Product(shop.Result);

            _context.Products.Add(productItem);
            await SaveChangesAsync();
        }
        public async Task AddShop(ShopDto shopDtoItem)
        {
            var shop = _context.Shops.Add(shopDtoItem.Shop());
            await SaveChangesAsync();
            
        }
        async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

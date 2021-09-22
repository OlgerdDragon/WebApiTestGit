using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Data;
using WebApiTest.Models;

namespace WebApiTest.Services.AdminService
{
    public class AdminService: IAdminService
    {
        private readonly TowerContext _context;

        public AdminService(TowerContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.Select(i => new Product
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                ShopId = i.ShopId
            }).ToListAsync();
        }

        public async Task<List<Shop>> GetShopsAsync()
        {
            return await _context.Shops.Select(i => new Shop
            {
                Id = i.Id,
                Name = i.Name
            }).ToListAsync();
        }
        public async Task<Shop> FindShopAsync(long id)
        {
            return await _context.Shops.FindAsync(id);
        }
        public async Task<Product> FindProductAsync(long id)
        {
            return await _context.Products.FindAsync(id);
        }
        public void RemoveProduct(Product productItem)
        {
            _context.Products.Remove(productItem);
        }
        public void RemoveShop(Shop todoItem)
        {
            _context.Shops.Remove(todoItem);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void AddProduct(Product productItem)
        {
            var _productItem = new Product
            {
                Id = productItem.Id,
                Name = productItem.Name,
                Price = productItem.Price,
                ShopId = productItem.Price
            };
            _context.Products.Add(_productItem);
        }
        public void AddShop(Shop shopItem)
        {
            var _shopItem = new Shop
            {
                Id = shopItem.Id,
                Name = shopItem.Name,
            };
            _context.Shops.Add(_shopItem);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;
using WebApiTest.Services.FactoryService;

namespace WebApiTest.Services.AdminService
{
    public class AdminService: IAdminService
    {
        private readonly TowerContext _context;

        public AdminService(TowerContext context)
        {
            _context = context;
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
        public async Task<Shop> FindShopAsync(int id)
        {
            return await _context.Shops.FindAsync(id);
        }
        public async Task<Product> FindProductAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public void RemoveProduct(Product productItem)
        {
            _context.Products.Remove(productItem);
        }
        public void RemoveShop(Shop shopItem)
        {
            _context.Shops.Remove(shopItem);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void AddProduct(Product productItem)
        {
            _context.Products.Add(productItem);
        }
        public void AddShop(Shop shopItem)
        {
            _context.Shops.Add(shopItem);
        }
        private static ShopDto ItemToDTO(Shop todoItem) =>
           new ShopDto
           {
               Id = todoItem.Id,
               Name = todoItem.Name,
           };
    }
}

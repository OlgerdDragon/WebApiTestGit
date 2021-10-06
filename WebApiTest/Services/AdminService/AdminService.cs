using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;


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
        public async void RemoveProduct(Product productItem)
        {
            _context.Products.Remove(productItem);
            await SaveChangesAsync();
            
        }

        public async void RemoveShop(Shop shopItem)
        {
            _context.Shops.Remove(shopItem);
            await SaveChangesAsync();
            
        }
        public async void AddProduct(Product productItem)
        {
            _context.Products.Add(productItem);
            await SaveChangesAsync();
        }
        public async void AddShop(Shop shopItem)
        {
            var shopDto = _context.Shops.Add(shopItem);
            await SaveChangesAsync();
            
        }
        async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.AdminService
{
    public interface IAdminService
    {
        public Task<List<ProductDto>> GetProductsAsync();
        public Task<List<ShopDto>> GetShopsAsync();
        public Task<ShopDto> UpdateShopAsync(ShopDto newShop);
        public Task<ProductDto> UpdateProductAsync(ProductDto newProduct);
        public Task<Shop> FindShopAsync(int id);
        public Task<Product> FindProductAsync(int id);
        public Task<ActionResult<ShopDto>> GetShopAsync(int id);
        public Task<ActionResult<ProductDto>> GetProductAsync(int id);
        public Task<bool> RemoveProduct(int id);
        public Task<bool> RemoveShop(int id);
        public Task AddProduct(ProductDto productDtoItem);
        public Task AddShop(ShopDto shopDtoItem);
        
    }
}

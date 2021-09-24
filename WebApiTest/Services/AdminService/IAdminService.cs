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
        public Task<Shop> FindShopAsync(int id);
        public Task<Product> FindProductAsync(int id);
        public void RemoveProduct(Product productDtoItem);
        public void RemoveShop(Shop shopDtoItem);
        public Task<int> SaveChangesAsync();
        public void AddProduct(Product productDtoItem);
        public void AddShop(Shop shopDtoItem);
    }
}

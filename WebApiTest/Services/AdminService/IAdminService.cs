using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Services.AdminService
{
    public interface IAdminService
    {
        public Task<List<Product>> GetProductsAsync();
        public Task<List<Shop>> GetShopsAsync();
        public Task<Shop> FindShopAsync(long id);
        public Task<Product> FindProductAsync(long id);
        public void RemoveProduct(Product productItem);
        public void RemoveShop(Shop todoItem);
        public Task<int> SaveChangesAsync();
        public void AddProduct(Product todoItem);
        public void AddShop(Shop todoItem);
    }
}

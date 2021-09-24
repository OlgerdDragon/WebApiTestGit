using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;


namespace WebApiTest.Services.HusbandService
{
    public interface IHusbandService
    {
        public Task<List<WantedList>> GetWantedListAsync();
        public Task<List<Shop>> GetShopsForVisitAsync();
        public Task<List<Product>> GetProductsInShopAsync(int ShopId);
        public Task<Shop> FindShopAsync(int id);
        public Task<Product> FindProductAsync(string name);
    }
}

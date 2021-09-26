using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.HusbandService
{
    public interface IHusbandService
    {
        public Task<List<WantedListDto>> GetWantedListAsync();
        public Task<List<ShopDto>> GetShopsForVisitAsync();
        public Task<List<ProductDto>> GetProductsInShopAsync(int ShopId);
        public Task<Shop> FindShopAsync(int id);
        public Task<Product> FindProductAsync(string name);
    }
}

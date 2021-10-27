using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.HusbandService
{
    public interface IHusbandService
    {
        public Task<Result<List<WantedProductDto>>> GetWantedProductsAsync();
        public Task<Result<List<ShopDto>>> GetShopsForVisitAsync();
        public Task<Result<List<ProductDto>>> GetProductsInShopAsync(int ShopId);
        public Task<Result<Shop>> FindShopAsync(int id);
        public Task<Result<Product>> FindProductAsync(string name);
    }
}

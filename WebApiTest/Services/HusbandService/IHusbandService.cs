using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.HusbandService
{
    public interface IHusbandService
    {
        public Task<Result<List<WantedProductDto>>> GetWantedProductsAsync(string userLogin);
        public Task<Result<List<ShopDto>>> GetShopsForVisitAsync(string userLogin);
        public Task<Result<List<ProductDto>>> GetProductsInShopAsync(int ShopId, string userLogin);
    }
}

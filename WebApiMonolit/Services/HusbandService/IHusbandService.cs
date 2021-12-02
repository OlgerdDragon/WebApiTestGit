using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiMonolit.Data;
using WebApiMonolit.Models;
using WebApiMonolit.Models.Dto;

namespace WebApiMonolit.Services.HusbandService
{
    public interface IHusbandService
    {
        public Task<Result<List<WantedProductDto>>> GetWantedProductsAsync(string userLogin);
        public Task<Result<List<ShopDto>>> GetShopsForVisitAsync(string userLogin);
        public Task<Result<List<ProductDto>>> GetProductsInShopAsync(int ShopId, string userLogin);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiGeneralGrpc.Data;
using WebApiGeneralGrpc.Models;
using WebApiGeneralGrpc.Models.Dto;

namespace WebApiGeneralGrpc.Services.HusbandService
{
    public interface IHusbandService
    {
        public Task<Result<List<WantedProductDto>>> GetWantedProductsAsync(string userLogin);
        public Task<Result<List<ShopDto>>> GetShopsForVisitAsync(string userLogin);
        public Task<Result<List<ProductDto>>> GetProductsInShopAsync(int ShopId, string userLogin);
    }
}

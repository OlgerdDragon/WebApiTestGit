using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.WifeService
{
    public interface IWifeService
    {
        public Task<Result<List<WantedProductDto>>> GetWantedProductsAsync();
        public Task<Result<int>> GetTotalAmountWantedProductsAsync();
        public Task<Result<WantedProductDto>> AddProduct(int id, string userLogin);
        public Task<Result<ActionResult<WantedProductDto>>> GetWantedProductItemAsync(int id);
        public Task<Result<bool>> RemoveWantedProduct(int id, string userLogin);
        public Task<Result<bool>> RemoveAllWantedProducts(string userLogin);
    }
}

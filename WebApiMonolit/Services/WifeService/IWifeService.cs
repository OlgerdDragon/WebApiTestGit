using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiMonolit.Data;
using WebApiMonolit.Models;
using WebApiMonolit.Models.Dto;

namespace WebApiMonolit.Services.WifeService
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

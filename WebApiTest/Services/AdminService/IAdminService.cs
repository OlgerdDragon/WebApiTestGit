using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiGeneralGrpc.Data;
using WebApiGeneralGrpc.Models;
using WebApiGeneralGrpc.Models.Dto;

namespace WebApiGeneralGrpc.Services.AdminService
{
    public interface IAdminService
    {
        public Task<Result<List<ProductDto>>> GetProductsAsync();
        public Task<Result<List<ShopDto>>> GetShopsAsync();
        public Task<Result<ShopDto>> UpdateShopAsync(ShopDto newShop, string userLogin);
        public Task<Result<ProductDto>> UpdateProductAsync(ProductDto newProduct, string userLogin);
        public Task<Result<ActionResult<ShopDto>>> GetShopAsync(int id);
        public Task<Result<ActionResult<ProductDto>>> GetProductAsync(int id);
        public Task<Result<bool>> RemoveProduct(int id, string userLogin);
        public Task<Result<bool>> RemoveShop(int id, string userLogin);
        public Task<Result<bool>> AddProduct(ProductDto productDtoItem, string userLogin);
        public Task<Result<bool>> AddShop(ShopDto shopDtoItem, string userLogin);
        
    }
}

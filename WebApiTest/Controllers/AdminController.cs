using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Services.AdminService;
using WebApiTest.Models.Dto;

using Microsoft.AspNetCore.Authorization;

namespace WebApiTest.Controllers
{
    [Authorize(Roles = "admin")]

    public class AdminController : APIControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        
        [HttpGet("Hello")]
        public string Get()
        {
            return "Hello Admin!";
        }

        [HttpGet("Shops")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShopItems() => await _adminService.GetShopsAsync();

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductItems() => await _adminService.GetProductsAsync();

        [HttpGet("Shop/{id}")]
        public async Task<ActionResult<ShopDto>> GetShopItem(int id) => await _adminService.GetShopAsync(id) ?? NotFound();

        [HttpGet("Product/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductItem(int id) => await _adminService.GetProductAsync(id) ?? NotFound();

        [HttpDelete("Shop/{id}")]
        public async Task<IActionResult> DeleteShopItem(int id)
        {
            if (await _adminService.RemoveShop(id))
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteProductItem(int id)
        {
            if (await _adminService.RemoveProduct(id))
            {
                return  NoContent();
            }
            return NotFound();
        }
        
        [HttpPut("Shop")]
        public async Task<ActionResult<ShopDto>> PutShopItem(ShopDto shopItemDto)
        {
            await _adminService.UpdateShopAsync(shopItemDto);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopItemDto.Id },
            shopItemDto);
        }
        [HttpPut("Product")]
        public async Task<ActionResult<ProductDto>> PutProductItem(ProductDto productItemDto)
        {
            await _adminService.UpdateProductAsync(productItemDto);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = productItemDto.Id },
            productItemDto);
        }

        [HttpPost("Product")]
        public async Task<ActionResult<ProductDto>> AddProductItem(ProductDto productDtoItem)
        {
            await _adminService.AddProduct(productDtoItem);

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productDtoItem.Id },
                productDtoItem);
        }
        [HttpPost("Shop")]
        public async Task<ActionResult<ShopDto>> AddShopItem(ShopDto shopDtoItem)
        { 
            await _adminService.AddShop(shopDtoItem);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItem.Id },
            shopDtoItem);
        }
        
        
    }
}

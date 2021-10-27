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
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShopItems()
        {
            var shopItems = await _adminService.GetShopsAsync();
            return shopItems.Element;
        } 

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductItems() 
        {
            var productItems = await _adminService.GetProductsAsync();
            return productItems.Element;
        }

        [HttpGet("Shop/{id}")]
        public async Task<ActionResult<ShopDto>> GetShopItem(int id)
        {
            var shopItem = await _adminService.GetShopAsync(id);
            return shopItem.Element ?? NotFound();
        }

        [HttpGet("Product/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductItem(int id)
        {
            var productItem = await _adminService.GetProductAsync(id);
            return productItem.Element ?? NotFound();
        }

        [HttpDelete("Shop/{id}")]
        public async Task<IActionResult> DeleteShopItem(int id)
        {
            var shopItem = await _adminService.RemoveShop(id);
            if (shopItem.Element)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteProductItem(int id)
        {
            var productItem = await _adminService.RemoveProduct(id);
            if (productItem.Element)
            {
                return  NoContent();
            }
            return NotFound();
        }
        
        [HttpPut("Shop")]
        public async Task<ActionResult<ShopDto>> PutShopItem(ShopDto shopDtoItem)
        {
            var shopItem = await _adminService.UpdateShopAsync(shopDtoItem);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItem.Id },
            shopDtoItem);
        }
        [HttpPut("Product")]
        public async Task<ActionResult<ProductDto>> PutProductItem(ProductDto productDtoItem)
        {
            var productItem = await _adminService.UpdateProductAsync(productDtoItem);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = productDtoItem.Id },
            productDtoItem);
        }

        [HttpPost("Product")]
        public async Task<ActionResult<ProductDto>> AddProductItem(ProductDto productDtoItem)
        {
            var product = await _adminService.AddProduct(productDtoItem);

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productDtoItem.Id },
                productDtoItem);
        }
        [HttpPost("Shop")]
        public async Task<ActionResult<ShopDto>> AddShopItem(ShopDto shopDtoItem)
        { 
            var shop = await _adminService.AddShop(shopDtoItem);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItem.Id },
            shopDtoItem);
        }
        
        
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Services.AdminService;
using WebApiTest.Models.Dto;

using Microsoft.AspNetCore.Authorization;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
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

        [HttpGet("shops")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShopItems() => await _adminService.GetShopsAsync();

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductItems() => await _adminService.GetProductsAsync();

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteProductItem(int id)
        {
            var productItem = await _adminService.FindProductAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }
            _adminService.RemoveProduct(productItem);
            return NoContent();
        }
        [HttpDelete("shop/{id}")]
        public async Task<IActionResult> DeleteShopItem(int id)
        {
            var shopItem = await _adminService.FindShopAsync(id);

            if (shopItem == null)
            {
                return NotFound();
            }
            _adminService.RemoveShop(shopItem);
            return NoContent();
        }
        [HttpGet("product/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductItem(int id)
        {
            var productItem = await _adminService.FindProductAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }

            return ProductDto.ItemProductDTO(productItem);
        }
        [HttpPut("shop")]
        public async Task<ActionResult<ShopDto>> PutShopItem(ShopDto shopItemDto)
        {
            await _adminService.UpdateShopAsync(shopItemDto);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopItemDto.Id },
            shopItemDto);
        }
        [HttpPut("product")]
        public async Task<ActionResult<ProductDto>> PutProductItem(ProductDto productItemDto)
        {
            await _adminService.UpdateProductAsync(productItemDto);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = productItemDto.Id },
            productItemDto);
        }
        [HttpGet("shop/{id}")]
        public async Task<ActionResult<ShopDto>> GetShopItem(int id)
        {
            var shopItem = await _adminService.FindShopAsync(id);

            if (shopItem == null)
            {
                return NotFound();
            }

            return ShopDto.ItemShopDTO(shopItem);
        }
        [HttpPost("product")]
        public async Task<ActionResult<ProductDto>> AddProductItem(ProductDto productItemDto)
        {
            var shop = _adminService.FindShopAsync(productItemDto.ShopId);
            var productItem = productItemDto.Product(shop.Result);
            
             _adminService.AddProduct(productItem);

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productItem.Id },
                productItemDto);
        }
        [HttpPost("shop")]
        public async Task<ActionResult<ShopDto>> AddShopItem(ShopDto shopItemDto)
        { 
            var shopItem = shopItemDto.Shop();
            
             _adminService.AddShop(shopItem);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopItem.Id },
            shopItemDto);
        }
        
        
    }
}

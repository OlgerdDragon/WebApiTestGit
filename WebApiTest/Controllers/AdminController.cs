using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Services.AdminService;
using WebApiTest.Models.Dto;
using System.Net.Http;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using Grpc.Net.Client;
using AdminGrpcService;

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

        [HttpGet("ShopsM")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShopItemsM()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var husbandService = new AdminGreeter (channel);

            var shopItems = await _adminService.GetShopsAsync();
            if (!shopItems.Successfully)
                return BadRequest(shopItems.ExceptionMessage);
            return shopItems.Element;
        }
        [HttpGet("Shops")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShopItems()
        {
            
            var shopItems = await _adminService.GetShopsAsync();
            if (!shopItems.Successfully) 
                return BadRequest(shopItems.ExceptionMessage);
            return shopItems.Element;
        } 

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductItems() 
        {
            var productItems = await _adminService.GetProductsAsync();
            if (!productItems.Successfully) 
                return BadRequest(productItems.ExceptionMessage);
            return productItems.Element;
        }

        [HttpGet("Shop/{id}")]
        public async Task<ActionResult<ShopDto>> GetShopItem(int id)
        {
            var shopItem = await _adminService.GetShopAsync(id);
            if (!shopItem.Successfully) 
                return BadRequest(shopItem.ExceptionMessage);
            return shopItem.Element ?? NotFound();
        }

        [HttpGet("Product/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductItem(int id)
        {
            var productItem = await _adminService.GetProductAsync(id);
            if (!productItem.Successfully) 
                return BadRequest(productItem.ExceptionMessage);
            return productItem.Element ?? NotFound();
        }

        [HttpDelete("Shop/{id}")]
        public async Task<IActionResult> DeleteShopItem(int id)
        {
            var shopItem = await _adminService.RemoveShop(id, userLogin);
            if (!shopItem.Successfully) 
                return BadRequest(shopItem.ExceptionMessage);
            if (shopItem.Element)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteProductItem(int id)
        {
            var productItem = await _adminService.RemoveProduct(id, userLogin);
            if (!productItem.Successfully) 
                return BadRequest(productItem.ExceptionMessage);
            if (productItem.Element)
            {
                return  NoContent();
            }
            return NotFound();
        }
        
        [HttpPut("Shop")]
        public async Task<ActionResult<ShopDto>> PutShopItem(ShopDto shopDtoItem)
        {
            var shopItem = await _adminService.UpdateShopAsync(shopDtoItem, userLogin);
            if (shopItem.Element.Id == 0)
                return BadRequest(Json(shopItem.Element).Value);
            if (!shopItem.Successfully) 
                return BadRequest(shopItem.ExceptionMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItem.Id },
            shopDtoItem);
        }
        [HttpPut("Product")]
        public async Task<ActionResult<ProductDto>> PutProductItem(ProductDto productDtoItem)
        {
            var productItem = await _adminService.UpdateProductAsync(productDtoItem, userLogin);
            if (productItem.Element.Id == 0)
                return BadRequest(Json(productItem.Element).Value);
            if (!productItem.Successfully) 
                return BadRequest(productItem.ExceptionMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = productDtoItem.Id },
            productDtoItem);
        }

        [HttpPost("Product")]
        public async Task<ActionResult<ProductDto>> AddProductItem(ProductDto productDtoItem)
        {
            var product = await _adminService.AddProduct(productDtoItem, userLogin);
            if (!product.Element)
                return BadRequest();
            if (!product.Successfully) 
                return BadRequest(product.ExceptionMessage);

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productDtoItem.Id },
                productDtoItem);
        }
        [HttpPost("Shop")]
        public async Task<ActionResult<ShopDto>> AddShopItem(ShopDto shopDtoItem)
        { 
            var shop = await _adminService.AddShop(shopDtoItem, userLogin);
            if (!shop.Successfully) 
                return BadRequest(shop.ExceptionMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItem.Id },
            shopDtoItem);
        }
        
        
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiGeneralGrpc.Services.AdminService;
using WebApiGeneralGrpc.Models.Dto;
using System.Net.Http;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using Grpc.Net.Client;
using AdminGrpcService;

namespace WebApiGeneralGrpc.Controllers
{
    //[Authorize(Roles = "admin")]

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
        public async Task<ActionResult<ListOfShopDto>> GetShopItemsM()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var shopItems = await adminService.GetShopsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!shopItems.Successfully)
                return BadRequest(shopItems.ErrorMessage);
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
        [HttpGet("ProductsM")]
        public async Task<ActionResult<ListOfProductDto>> GetProductItemsM()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var productItems = await adminService.GetProductsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!productItems.Successfully)
                return BadRequest(productItems.ErrorMessage);
            return productItems.Element;
        }
        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductItems() 
        {
            var productItems = await _adminService.GetProductsAsync();
            if (!productItems.Successfully) 
                return BadRequest(productItems.ExceptionMessage);
            return productItems.Element;
        }
        [HttpGet("ShopM/{id}")]
        public async Task<ActionResult<ShopDtoMessage>> GetShopItemM(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var shopItem = await adminService.GetShopAsync(new ItemRequest { Id = id, UserLogin = userLogin});
            if (!shopItem.Successfully)
                return BadRequest(shopItem.ErrorMessage);
            if(shopItem.Element.Id == 0)
                return NotFound();
            return shopItem.Element;
        }
        [HttpGet("Shop/{id}")]
        public async Task<ActionResult<ShopDto>> GetShopItem(int id)
        {
            var shopItem = await _adminService.GetShopAsync(id);
            if (!shopItem.Successfully) 
                return BadRequest(shopItem.ExceptionMessage);
            return shopItem.Element ?? NotFound();
        }
        [HttpGet("ProductM/{id}")]
        public async Task<ActionResult<ProductDtoMessage>> GetProductItemM(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var productItem = await adminService.GetProductAsync(new ItemRequest { Id = id, UserLogin = userLogin });
            if (!productItem.Successfully)
                return BadRequest(productItem.ErrorMessage);
            if (productItem.Element.Id == 0)
                return NotFound();
            return productItem.Element;
        }
        [HttpGet("Product/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductItem(int id)
        {
            var productItem = await _adminService.GetProductAsync(id);
            if (!productItem.Successfully) 
                return BadRequest(productItem.ExceptionMessage);
            return productItem.Element ?? NotFound();
        }
        [HttpDelete("ShopM/{id}")]
        public async Task<IActionResult> DeleteShopItemM(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var shopItem = await adminService.RemoveShopAsync(new ItemRequest { Id = id, UserLogin = userLogin });
            if (!shopItem.Successfully)
                return BadRequest(shopItem.ErrorMessage);
            if (shopItem.Element)
            {
                return NoContent();
            }
            return NotFound();
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
        [HttpDelete("ProductM/{id}")]
        public async Task<IActionResult> DeleteProductItemM(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var productItem = await adminService.RemoveProductAsync(new ItemRequest { Id = id, UserLogin = userLogin });
            if (!productItem.Successfully)
                return BadRequest(productItem.ErrorMessage);
            if (productItem.Element)
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
        [HttpPut("ShopM")]
        public async Task<ActionResult<ShopDto>> PutShopItemM(ShopDtoMessage shopDtoItemMessage)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);
            
            var shopItem = await adminService.UpdateShopAsync(new ShopRequest {ShopDtoMessage = shopDtoItemMessage, UserLogin = userLogin});
            if (shopItem.Element.Id == 0)
                return BadRequest(Json(shopItem.Element).Value);
            if (!shopItem.Successfully)
                return BadRequest(shopItem.ErrorMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItemMessage.Id },
            shopDtoItemMessage);
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
        [HttpPut("ProductM")]
        public async Task<ActionResult<ProductDto>> PutProductItemM(ProductDtoMessage productDtoItemMessage)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var productItem = await adminService.UpdateProductAsync(new ProductRequest { ProductDtoMessage = productDtoItemMessage, UserLogin = userLogin });
            if (productItem.Element.Id == 0)
                return BadRequest(Json(productItem.Element).Value);
            if (!productItem.Successfully)
                return BadRequest(productItem.ErrorMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = productDtoItemMessage.Id },
            productDtoItemMessage);
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
        [HttpPost("ProductM")]
        public async Task<ActionResult<ProductDto>> AddProductItemM(ProductDtoMessage productDtoItemMessage)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var product = await adminService.AddProductAsync(new ProductRequest { ProductDtoMessage = productDtoItemMessage, UserLogin = userLogin });
            if (!product.Element)
                return BadRequest();
            if (!product.Successfully)
                return BadRequest(product.ErrorMessage);

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productDtoItemMessage.Id },
                productDtoItemMessage);
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
        [HttpPost("ShopM")]
        public async Task<ActionResult<ShopDto>> AddShopItemM(ShopDtoMessage shopDtoItemMessage)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var adminService = new AdminGreeter.AdminGreeterClient(channel);

            var shop = await adminService.AddShopAsync(new ShopRequest { ShopDtoMessage = shopDtoItemMessage, UserLogin = userLogin });
            if (!shop.Successfully)
                return BadRequest(shop.ErrorMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItemMessage.Id },
            shopDtoItemMessage);
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

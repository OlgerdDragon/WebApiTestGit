using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AdminGrpcService;
using WebApiGeneralGrpc.Models.Dto;

using WebApiGeneralGrpc.Services.AdminService;

namespace WebApiGeneralGrpc.Controllers
{
    //[Authorize(Roles = "admin")]

    public class AdminController : APIControllerBase
    {
        private readonly AdminGreeter.AdminGreeterClient _adminServiceClient;

        public AdminController(IAdminServiceFactory adminServiceFactory)
        {
            _adminServiceClient = adminServiceFactory.GetGrpcClient();
        }

        [HttpGet("Hello")]
        public string Get()
        {
            return "Hello Admin!";
        }

        [HttpGet("Shops")]
        public async Task<ActionResult<ListOfShopDto>> GetShopItems()
        {
            var shopItems = await _adminServiceClient.GetShopsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!shopItems.Successfully)
                return BadRequest(shopItems.ErrorMessage);
            return Ok(shopItems.Element);
        }
       
        [HttpGet("Products")]
        public async Task<ActionResult<ListOfProductDto>> GetProductItemsM()
        {

            var productItems = await _adminServiceClient.GetProductsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!productItems.Successfully)
                return BadRequest(productItems.ErrorMessage);
            return Ok(productItems.Element);
        }
     
        [HttpGet("Shop/{id}")]
        public async Task<ActionResult<ShopDtoMessage>> GetShopItem(int id)
        {
            var shopItem = await _adminServiceClient.GetShopAsync(new ItemRequest { Id = id, UserLogin = userLogin});
            if (!shopItem.Successfully)
                return BadRequest(shopItem.ErrorMessage);
            if(shopItem.Element.Id == 0)
                return NotFound();
            return Ok(shopItem.Element);
        }
        
        [HttpGet("Product/{id}")]
        public async Task<ActionResult<ProductDtoMessage>> GetProductItem(int id)
        {
            var productItem = await _adminServiceClient.GetProductAsync(new ItemRequest { Id = id, UserLogin = userLogin });
            if (!productItem.Successfully)
                return BadRequest(productItem.ErrorMessage);
            if (productItem.Element.Id == 0)
                return NotFound();
            return Ok(productItem.Element);
        }
        
        [HttpDelete("Shop/{id}")]
        public async Task<IActionResult> DeleteShopItemM(int id)
        {
            var shopItem = await _adminServiceClient.RemoveShopAsync(new ItemRequest { Id = id, UserLogin = userLogin });
            if (!shopItem.Successfully)
                return BadRequest(shopItem.ErrorMessage);
            if (shopItem.Element)
            {
                return NoContent();
            }
            return NotFound();
        }
        
        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteProductItem(int id)
        {
            var productItem = await _adminServiceClient.RemoveProductAsync(new ItemRequest { Id = id, UserLogin = userLogin });
            if (!productItem.Successfully)
                return BadRequest(productItem.ErrorMessage);
            if (productItem.Element)
            {
                return NoContent();
            }
            return NotFound();
        }
      
        [HttpPut("Shop")]
        public async Task<ActionResult<ShopDto>> PutShopItemM(ShopDtoMessage shopDtoItemMessage)
        {
            var shopItem = await _adminServiceClient.UpdateShopAsync(new ShopRequest {ShopDtoMessage = shopDtoItemMessage, UserLogin = userLogin});
            if (shopItem.Element.Id == 0)
                return BadRequest(Json(shopItem.Element).Value);
            if (!shopItem.Successfully)
                return BadRequest(shopItem.ErrorMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItemMessage.Id },
            shopDtoItemMessage);
        }
       
        [HttpPut("Product")]
        public async Task<ActionResult<ProductDto>> PutProductItem(ProductDtoMessage productDtoItemMessage)
        {
            var productItem = await _adminServiceClient.UpdateProductAsync(new ProductRequest { ProductDtoMessage = productDtoItemMessage, UserLogin = userLogin });
            if (productItem.Element.Id == 0)
                return BadRequest(Json(productItem.Element).Value);
            if (!productItem.Successfully)
                return BadRequest(productItem.ErrorMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = productDtoItemMessage.Id },
            productDtoItemMessage);
        }
        
        [HttpPost("Product")]
        public async Task<ActionResult<ProductDto>> AddProductItem(ProductDtoMessage productDtoItemMessage)
        {
            var product = await _adminServiceClient.AddProductAsync(new ProductRequest { ProductDtoMessage = productDtoItemMessage, UserLogin = userLogin });
            if (!product.Element)
                return BadRequest();
            if (!product.Successfully)
                return BadRequest(product.ErrorMessage);

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productDtoItemMessage.Id },
                productDtoItemMessage);
        }
        
        [HttpPost("Shop")]
        public async Task<ActionResult<ShopDto>> AddShopItem(ShopDtoMessage shopDtoItemMessage)
        {
            var shop = await _adminServiceClient.AddShopAsync(new ShopRequest { ShopDtoMessage = shopDtoItemMessage, UserLogin = userLogin });
            if (!shop.Successfully)
                return BadRequest(shop.ErrorMessage);

            return CreatedAtAction(
            nameof(GetShopItem),
            new { id = shopDtoItemMessage.Id },
            shopDtoItemMessage);
        }
       
        
        
    }
}

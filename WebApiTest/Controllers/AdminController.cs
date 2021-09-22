using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Services.AdminService;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
       
        [HttpGet]
        public string Get()
        {
            return "Hello Admin!";
        }

        [HttpGet]
        [Route("GetShopsList")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetShopItems() => await _adminService.GetShopsAsync();

        [HttpGet]
        [Route("GetProductList")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductItems() => await _adminService.GetProductsAsync();

        [HttpDelete("{id}")]
        [Route("RemoveProduct")]
        public async Task<IActionResult> DeleteProductItem(long id)
        {
            var productItem = await _adminService.FindProductAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }

            _adminService.RemoveProduct(productItem);
            await _adminService.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Route("RemoveShop")]
        public async Task<IActionResult> DeleteShopItem(long id)
        {
            var shopItem = await _adminService.FindShopAsync(id);

            if (shopItem == null)
            {
                return NotFound();
            }

            _adminService.RemoveShop(shopItem);
            await _adminService.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("{id}")]
        [Route("GetProduct")]
        public async Task<ActionResult<Product>> GetProductItem(long id)
        {
            var productItem = await _adminService.FindProductAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }

            return productItem;
        }
        [HttpGet("{id}")]
        [Route("GetProduct")]
        public async Task<ActionResult<Shop>> GetShopItem(long id)
        {
            var shopItem = await _adminService.FindShopAsync(id);

            if (shopItem == null)
            {
                return NotFound();
            }

            return shopItem;
        }
        [HttpPost]
        [Route("PostProduct")]
        public async Task<ActionResult<Product>> CreateProductItem(Product productItem)
        {
            _adminService.AddProduct(productItem);
            await _adminService.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productItem.Id },
                productItem);
        }
        [HttpPost]
        [Route("PostShop")]
        public async Task<ActionResult<Shop>> CreateShopItem(Shop shopItem)
        {
            _adminService.AddShop(shopItem);
            await _adminService.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetShopItem),
                new { id = shopItem.Id },
                shopItem);
        }
    }
}

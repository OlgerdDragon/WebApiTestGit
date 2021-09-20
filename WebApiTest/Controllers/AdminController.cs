using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiTest.Data.Interface;
using WebApiTest.Models;
using WebApiTest.Services.AdminService;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var todoItem = await _adminService.FindProductAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _adminService.RemoveProduct(todoItem);
            await _adminService.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Route("RemoveShop")]
        public async Task<IActionResult> DeleteShopItem(long id)
        {
            var todoItem = await _adminService.FindShopAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _adminService.RemoveShop(todoItem);
            await _adminService.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("{id}")]
        [Route("GetProduct")]
        public async Task<ActionResult<Product>> GetProductItem(long id)
        {
            var todoItem = await _adminService.FindProductAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        [HttpGet("{id}")]
        [Route("GetProduct")]
        public async Task<ActionResult<Shop>> GetShopItem(long id)
        {
            var todoItem = await _adminService.FindShopAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        [HttpPost]
        [Route("PostProduct")]
        public async Task<ActionResult<Product>> CreateProductItem(Product productItem)
        {
            var _productItem = new Product
            {
                Id = productItem.Id,
                Name = productItem.Name,
                Price = productItem.Price,
                ShopId = productItem.Price
            };

            _adminService.AddProduct(_productItem);
            await _adminService.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = _productItem.Id },
                _productItem);
        }
        [HttpPost]
        [Route("PostShop")]
        public async Task<ActionResult<Shop>> CreateShopItem(Shop productItem)
        {
            var _productItem = new Shop
            {
                Id = productItem.Id,
                Name = productItem.Name,
            };

            _adminService.AddShop(_productItem);
            await _adminService.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = _productItem.Id },
                _productItem);
        }
    }
}

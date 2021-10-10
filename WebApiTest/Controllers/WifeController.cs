using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Models.Dto;
using WebApiTest.Services.WifeService;

namespace WebApiTest.Controllers
{
    [Authorize(Roles = "wife")]
    public class WifeController : APIControllerBase
    {
        private readonly IWifeService _wifeService;

        public WifeController(IWifeService wifeService)
        {
            _wifeService = wifeService;
        }
        
        [HttpGet]
        public string Get()
        {
            return "Hello Wife!";
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<WantedProduct>>> GetWantedProducts() => await _wifeService.GetWantedProductsAsync();

        [HttpPost("Product")]
        public async Task<ActionResult<WantedProductDto>> CreateProductItem(WantedProductDto wantedProductItem)
        {
            await _wifeService.AddProduct(wantedProductItem);

            return CreatedAtAction(
                nameof(GetWantedProductItem),
                new { id = wantedProductItem.Id },
                wantedProductItem);
        }
        
        [HttpGet("Product/{id}")]
        public async Task<ActionResult<WantedProduct>> GetWantedProductItem(int id)
        {
            var wantedProductItem = await _wifeService.FindWantedProductAsync(id);

            if (wantedProductItem == null)
            {
                return NotFound();
            }

            return wantedProductItem;
        }

        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteWantedProductItem(int id)
        {
            var productItem = await _wifeService.FindWantedProductAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }

            
            await _wifeService.RemoveWantedProduct(productItem);

            return NoContent();
        }
        [HttpDelete("Products")]
        public async Task<IActionResult> DeleteAllProductItem()
        {
            
            await _wifeService.RemoveAllWantedProducts();  

            return NoContent();
        }

    }
}

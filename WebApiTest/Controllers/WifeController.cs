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

        [HttpGet("Product/{id}")]
        public async Task<ActionResult<WantedProductDto>> GetWantedProductItem(int id) => await _wifeService.GetWantedProductItemAsync(id) ?? NotFound();

        [HttpPost("Product")]
        public async Task<ActionResult<WantedProductDto>> CreateProductItem(WantedProductDto wantedProductItem)
        {
            await _wifeService.AddProduct(wantedProductItem);

            return CreatedAtAction(
                nameof(GetWantedProductItem),
                new { id = wantedProductItem.Id },
                wantedProductItem);
        }

        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteWantedProductItem(int id)
        {
            if (await _wifeService.RemoveWantedProduct(id))
            {
                return NoContent(); 
            }
            return NotFound();
        }

        [HttpDelete("Products")]
        public async Task<IActionResult> DeleteAllProductItem()
        {
            await _wifeService.RemoveAllWantedProducts();  
            return NoContent();
        }

    }
}

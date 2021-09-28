using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Models.Dto;
using WebApiTest.Services.WifeService;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WifeController : Controller
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

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<WantedProduct>>> GetWantedList() => await _wifeService.GetWantedProductsAsync();

        [HttpPost("product")]
        public async Task<ActionResult<WantedProductDto>> CreateProductItem(WantedProductDto wantedListItem)
        {
            _wifeService.AddProduct(wantedListItem);
            await _wifeService.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWantedListItem),
                new { id = wantedListItem.Id },
                wantedListItem);
        }
        
        [HttpGet("product/{id}")]
        public async Task<ActionResult<WantedProduct>> GetWantedListItem(int id)
        {
            var wantedListItem = await _wifeService.FindWantedProductAsync(id);

            if (wantedListItem == null)
            {
                return NotFound();
            }

            return wantedListItem;
        }

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteWantedListItem(int id)
        {
            var productItem = await _wifeService.FindWantedProductAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }

            _wifeService.RemoveWantedProduct(productItem);
            await _wifeService.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("products")]
        public async Task<IActionResult> DeleteAllProductItem()
        {
            _wifeService.RemoveAllWantedProducts();
            await _wifeService.SaveChangesAsync();

            return NoContent();
        }

    }
}

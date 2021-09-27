using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
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
        public async Task<ActionResult<IEnumerable<WantedList>>> GetWantedList() => await _wifeService.GetWantedListAsync();

        [HttpPost("product")]
        public async Task<ActionResult<WantedList>> CreateProductItem(WantedList wantedListItem)
        {
            _wifeService.AddProduct(wantedListItem);
            await _wifeService.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWantedListItem),
                new { id = wantedListItem.Id },
                wantedListItem);
        }
        
        [HttpGet("product/{id}")]
        public async Task<ActionResult<WantedList>> GetWantedListItem(int id)
        {
            var wantedListItem = await _wifeService.FindWantedListAsync(id);

            if (wantedListItem == null)
            {
                return NotFound();
            }

            return wantedListItem;
        }

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteWantedListItem(int id)
        {
            var productItem = await _wifeService.FindWantedListAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }

            _wifeService.RemoveWantedList(productItem);
            await _wifeService.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("products")]
        public async Task<IActionResult> DeleteAllProductItem()
        {
            _wifeService.RemoveAllWantedList();
            await _wifeService.SaveChangesAsync();

            return NoContent();
        }

    }
}

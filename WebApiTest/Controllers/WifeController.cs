using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Services.WifeService;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/wife/[controller]")]
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

        [HttpPost]
        [Route("CreatedProductList")]
        public string CreatedProductList() 
        {
            
            return "I done CreatedProductList"; 
        }
        [HttpPost]
        [Route("AddProduct")]
        public async Task<ActionResult<WantedList>> CreateProductItem(WantedList wantedListItem)
        {
            _wifeService.AddProduct(wantedListItem);
            await _wifeService.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWantedListItem),
                new { id = wantedListItem.Id },
                wantedListItem);
        }
        [HttpDelete]
        [Route("DeletedProduct")]
        public string GetNededProduct() { return "I done GetNededProduct"; }

        [HttpGet("{id}")]
        [Route("GetWantedList")]
        public async Task<ActionResult<WantedList>> GetWantedListItem(long id)
        {
            var wantedListItem = await _wifeService.FindWantedListAsync(id);

            if (wantedListItem == null)
            {
                return NotFound();
            }

            return wantedListItem;
        }
    }
}

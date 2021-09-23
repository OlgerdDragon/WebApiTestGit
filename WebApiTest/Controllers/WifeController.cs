﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("GetStatusWantedList")]
        public async Task<ActionResult<IEnumerable<WantedList>>> GetShopItems() => await _wifeService.GetWantedListAsync();

        [HttpPost]
        [Route("CreatedProductList")]
        public string CreatedProductList() 
        {
            
            return "I done CreatedProductList"; 
        }
        [HttpPost]
        [Route("AddWantedProduct")]
        public async Task<ActionResult<WantedList>> CreateProductItem(WantedList wantedListItem)
        {
            _wifeService.AddProduct(wantedListItem);
            await _wifeService.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWantedListItem),
                new { id = wantedListItem.Id },
                wantedListItem);
        }
        
        [HttpGet("{id}")]
        [Route("GetWantedListProduct")]
        public async Task<ActionResult<WantedList>> GetWantedListItem(long id)
        {
            var wantedListItem = await _wifeService.FindWantedListAsync(id);

            if (wantedListItem == null)
            {
                return NotFound();
            }

            return wantedListItem;
        }

        [HttpDelete("{id}")]
        [Route("RemoveWantedProduct")]
        public async Task<IActionResult> DeleteProductItem(long id)
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
        [HttpDelete]
        [Route("CreateWantedListProduct")]
        public async Task<IActionResult> DeleteAllProductItem()
        {
            _wifeService.RemoveAllWantedList();
            await _wifeService.SaveChangesAsync();

            return NoContent();
        }

    }
}

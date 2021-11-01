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
        public async Task<ActionResult<IEnumerable<WantedProductDto>>> GetWantedProducts()
        {
            var wantedProducts = await _wifeService.GetWantedProductsAsync();
            if (!wantedProducts.Successfully) 
                return BadRequest(wantedProducts.ExceptionMessage);
            return wantedProducts.Element;
        }
        [HttpGet("Products/TotalAmount")]
        public async Task<ActionResult<string>> GetTotalAmountWantedProducts()
        {
            var totalAmount = await _wifeService.GetTotalAmountWantedProductsAsync();
            if (!totalAmount.Successfully) 
                return BadRequest(totalAmount.ExceptionMessage);
            return totalAmount.Element;
        }

        [HttpGet("Product/{id}")]
        public async Task<ActionResult<WantedProductDto>> GetWantedProductItem(int id)
        {
            var wantedProduct = await _wifeService.GetWantedProductItemAsync(id);
            if (!wantedProduct.Successfully) 
                return BadRequest(wantedProduct.ExceptionMessage);
            return wantedProduct.Element ?? NotFound();
        }

        [HttpPost("Product/{id}")]
        public async Task<ActionResult<WantedProductDto>> CreateWantedProductItem(int id)
        { 
            var product = await _wifeService.AddProduct(id, userLogin);
            if (!product.Successfully) 
                return BadRequest(product.ExceptionMessage);
            var wantedProductDtoItem = product.Element;

            return CreatedAtAction(
                nameof(GetWantedProductItem),
                new { id = wantedProductDtoItem.Id },
                wantedProductDtoItem);
        }

        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteWantedProductItem(int id)
        {
            var wantedProduct = await _wifeService.RemoveWantedProduct(id, userLogin);
            if (!wantedProduct.Successfully) 
                return BadRequest(wantedProduct.ExceptionMessage);

            if (wantedProduct.Element)
            {
                return NoContent(); 
            }
            return NotFound();
        }

        [HttpDelete("Products")]
        public async Task<IActionResult> DeleteAllProductItem()
        {
            var result = await _wifeService.RemoveAllWantedProducts(userLogin);
            if (!result.Successfully) 
                return BadRequest(result.ExceptionMessage);

            return NoContent();
        }

    }
}

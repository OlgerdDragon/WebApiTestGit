using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiGeneralGrpc.Models;
using WebApiGeneralGrpc.Models.Dto;
using WebApiGeneralGrpc.Services.WifeService;
using WifeGrpcService;

namespace WebApiGeneralGrpc.Controllers
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
        [HttpGet("ProductsM")]
        public async Task<ActionResult<ListOfWantedProductDto>> GetWantedProductsM()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeService = new WifeGreeter.WifeGreeterClient(channel);

            var wantedProducts = await wifeService.GetWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!wantedProducts.Successfully)
                return BadRequest(wantedProducts.ErrorMessage);
            return wantedProducts.Element;
        }
        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<WantedProductDto>>> GetWantedProducts()
        {
            var wantedProducts = await _wifeService.GetWantedProductsAsync();
            if (!wantedProducts.Successfully) 
                return BadRequest(wantedProducts.ExceptionMessage);
            return wantedProducts.Element;
        }
        [HttpGet("ProductsM/TotalAmount")]
        public async Task<ActionResult<int>> GetTotalAmountWantedProductsM()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeService = new WifeGreeter.WifeGreeterClient(channel);

            var totalAmount = await wifeService.GetTotalAmountWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!totalAmount.Successfully)
                return BadRequest(totalAmount.ErrorMessage);
            return totalAmount.Element;
        }
        [HttpGet("Products/TotalAmount")]
        public async Task<ActionResult<int>> GetTotalAmountWantedProducts()
        {
            var totalAmount = await _wifeService.GetTotalAmountWantedProductsAsync();
            if (!totalAmount.Successfully) 
                return BadRequest(totalAmount.ExceptionMessage);
            return totalAmount.Element;
        }
        [HttpGet("ProductM/{id}")]
        public async Task<ActionResult<WantedProductDtoMessage>> GetWantedProductItemM(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeService = new WifeGreeter.WifeGreeterClient(channel);

            var wantedProduct = await wifeService.GetWantedProductItemAsync(new ItemRequest() { Id = id, UserLogin = userLogin });
            if (!wantedProduct.Successfully)
                return BadRequest(wantedProduct.ErrorMessage);
            if (wantedProduct.Element.Id == 0)
                return NotFound();
            return wantedProduct.Element;
        }

        [HttpGet("Product/{id}")]
        public async Task<ActionResult<WantedProductDto>> GetWantedProductItem(int id)
        {
            var wantedProduct = await _wifeService.GetWantedProductItemAsync(id);
            if (!wantedProduct.Successfully) 
                return BadRequest(wantedProduct.ExceptionMessage);
            return wantedProduct.Element ?? NotFound();
        }
        [HttpPost("ProductM/{id}")]
        public async Task<ActionResult<WantedProductDtoMessage>> CreateWantedProductItemM(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeService = new WifeGreeter.WifeGreeterClient(channel);

            var product = await wifeService.AddProductAsync(new ItemRequest() { Id = id, UserLogin = userLogin });
            if (!product.Successfully)
                return BadRequest(product.ErrorMessage);
            var wantedProductDtoItem = product.Element;

            return CreatedAtAction(
                nameof(GetWantedProductItem),
                new { id = wantedProductDtoItem.Id },
                wantedProductDtoItem);
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
        [HttpDelete("ProductM/{id}")]
        public async Task<IActionResult> DeleteWantedProductItemM(int id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeService = new WifeGreeter.WifeGreeterClient(channel);

            var wantedProduct = await wifeService.RemoveWantedProductAsync(new ItemRequest() { Id = id, UserLogin = userLogin });
            if (!wantedProduct.Successfully)
                return BadRequest(wantedProduct.ErrorMessage);
            if (wantedProduct.Element)
                return NoContent();
            return NotFound();
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
        [HttpDelete("ProductsM")]
        public async Task<IActionResult> DeleteAllProductItemM()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeService = new WifeGreeter.WifeGreeterClient(channel);

            var result = await wifeService.RemoveAllWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!result.Successfully)
                return BadRequest(result.ErrorMessage);
            return NoContent();
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

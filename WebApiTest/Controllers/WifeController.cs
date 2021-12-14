using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiGeneralGrpc.Services.WifeService;
using WifeGrpcService;

namespace WebApiGeneralGrpc.Controllers
{
    //[Authorize(Roles = "wife")]
    public class WifeController : APIControllerBase
    {
        private readonly WifeGreeter.WifeGreeterClient _wifeServiceClient;

        public WifeController(IWifeServiceFactory wifeServiceFactory)
        {
            _wifeServiceClient = wifeServiceFactory.GetGrpcClient();
        }

        [HttpGet]
        public string Get()
        {
            return "Hello Wife!";
        }
        [HttpGet("Products")]
        public async Task<ActionResult<ListOfWantedProductDto>> GetWantedProducts()
        {
            var wantedProducts = await _wifeServiceClient.GetWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!wantedProducts.Successfully)
                return BadRequest(wantedProducts.ErrorMessage);
            return wantedProducts.Element;
        }
        
        [HttpGet("Products/TotalAmount")]
        public async Task<ActionResult<int>> GetTotalAmountWantedProducts()
        {
            var totalAmount = await _wifeServiceClient.GetTotalAmountWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!totalAmount.Successfully)
                return BadRequest(totalAmount.ErrorMessage);
            return totalAmount.Element;
        }
        
        [HttpGet("Product/{id}")]
        public async Task<ActionResult<WantedProductDtoMessage>> GetWantedProductItem(int id)
        {
            var wantedProduct = await _wifeServiceClient.GetWantedProductItemAsync(new ItemRequest() { Id = id, UserLogin = userLogin });
            if (!wantedProduct.Successfully)
                return BadRequest(wantedProduct.ErrorMessage);
            if (wantedProduct.Element.Id == 0)
                return NotFound();
            return wantedProduct.Element;
        }

        
        [HttpPost("Product/{id}")]
        public async Task<ActionResult<WantedProductDtoMessage>> CreateWantedProductItem(int id)
        {
            var product = await _wifeServiceClient.AddProductAsync(new ItemRequest() { Id = id, UserLogin = userLogin });
            if (!product.Successfully)
                return BadRequest(product.ErrorMessage);
            var wantedProductDtoItem = product.Element;

            return CreatedAtAction(
                nameof(GetWantedProductItem),
                new { id = wantedProductDtoItem.Id },
                wantedProductDtoItem);
        }
        
        [HttpDelete("Product/{id}")]
        public async Task<IActionResult> DeleteWantedProductItem(int id)
        {
            var wantedProduct = await _wifeServiceClient.RemoveWantedProductAsync(new ItemRequest() { Id = id, UserLogin = userLogin });
            if (!wantedProduct.Successfully)
                return BadRequest(wantedProduct.ErrorMessage);
            if (wantedProduct.Element)
                return NoContent();
            return NotFound();
        }
        
        [HttpDelete("Products")]
        public async Task<IActionResult> DeleteAllProductItem()
        {           
            var result = await _wifeServiceClient.RemoveAllWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!result.Successfully)
                return BadRequest(result.ErrorMessage);
            return NoContent();
        }
        

    }
}

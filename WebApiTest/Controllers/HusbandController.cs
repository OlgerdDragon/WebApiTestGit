using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models.Dto;
using WebApiTest.Services.HusbandService;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Authorize(Roles = "husband")]
    [Route("api/[controller]")]
    public class HusbandController : Controller
    {
        private readonly IHusbandService _husbandService;

        public HusbandController(IHusbandService husbandService)
        {
            _husbandService = husbandService;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello Husband!";
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<WantedProductDto>>> GetNeededProductList() => await _husbandService.GetWantedProductsAsync();
        [HttpGet("shops")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetNeededShopList() => await _husbandService.GetShopsForVisitAsync();

        [HttpGet("products-in-shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetNededProductListInShop(int shopId) => await _husbandService.GetProductsInShopAsync(shopId);
    }
}

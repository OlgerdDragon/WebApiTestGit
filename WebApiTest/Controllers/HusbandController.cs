using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models.Dto;
using WebApiTest.Services.HusbandService;

namespace WebApiTest.Controllers
{
    
    [Authorize(Roles = "husband")]
    
    public class HusbandController : APIControllerBase
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

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<WantedProductDto>>> GetNeededProductList() => await _husbandService.GetWantedProductsAsync();
        [HttpGet("Shops")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetNeededShopList() => await _husbandService.GetShopsForVisitAsync();

        [HttpGet("Products-in-shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetNededProductListInShop(int shopId) => await _husbandService.GetProductsInShopAsync(shopId);
    }
}

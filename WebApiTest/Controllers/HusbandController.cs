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
        public async Task<ActionResult<IEnumerable<WantedProductDto>>> GetNeededProductList()
        {
            var neededProductList = await _husbandService.GetWantedProductsAsync();
            if (!neededProductList.Successfully) return BadRequest();
            return neededProductList.Element;
        }
        [HttpGet("Shops")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetNeededShopList()
        {
            var neededShopList = await _husbandService.GetShopsForVisitAsync();
            if (!neededShopList.Successfully) return BadRequest();
            return neededShopList.Element;
        }

        [HttpGet("ProductsInShop/{shopId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetNededProductListInShop(int shopId)
        {
            var nededProductListInShop = await _husbandService.GetProductsInShopAsync(shopId);
            if (!nededProductListInShop.Successfully) return BadRequest();
            return nededProductListInShop.Element;
        }
}
}

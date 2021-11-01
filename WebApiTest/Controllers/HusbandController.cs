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
            var neededProductList = await _husbandService.GetWantedProductsAsync(userLogin);
            if (!neededProductList.Successfully) 
                return BadRequest(neededProductList.ExceptionMessage);
            return neededProductList.Element;
        }
        [HttpGet("Shops")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetNeededShopList()
        {
            var neededShopList = await _husbandService.GetShopsForVisitAsync(userLogin);
            if (!neededShopList.Successfully) 
                return BadRequest(neededShopList.ExceptionMessage);
            return neededShopList.Element;
        }

        [HttpGet("ProductsInShop/{shopId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetNededProductListInShop(int shopId)
        {
            var nededProductListInShop = await _husbandService.GetProductsInShopAsync(shopId, userLogin);
            if (!nededProductListInShop.Successfully) 
                return BadRequest(nededProductListInShop.ExceptionMessage);
            return nededProductListInShop.Element;
        }
}
}

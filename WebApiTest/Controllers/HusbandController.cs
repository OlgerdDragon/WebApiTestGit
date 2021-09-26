using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Models.Dto;
using WebApiTest.Services.HusbandService;

namespace WebApiTest.Controllers
{
    [ApiController]
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

        [HttpGet("ProductList")]
        public async Task<ActionResult<IEnumerable<WantedListDto>>> GetNeededProductList() => await _husbandService.GetWantedListAsync();
        [HttpGet("ShopList")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetNeededShopList() => await _husbandService.GetShopsForVisitAsync();

        [HttpGet("ProductListInShop/{shopId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetNededProductListInShop(int shopId) => await _husbandService.GetProductsInShopAsync(shopId);
    }
}

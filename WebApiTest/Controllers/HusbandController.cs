using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Services.HusbandService;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/husband/[controller]")]
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

        [HttpGet]
        [Route("GetNeededProductList")]
        public async Task<ActionResult<IEnumerable<WantedList>>> GetNeededProductList() => await _husbandService.GetWantedListAsync();
        [HttpGet]
        [Route("GetNeededShopList")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetNeededShopList() => await _husbandService.GetShopsForVisitAsync();

        [HttpGet("{shopId}")]
        [Route("GetNededProductListInShop")]
        public async Task<ActionResult<IEnumerable<Product>>> GetNededProductListInShop(int shopId) => await _husbandService.GetProductsInShopAsync(shopId);
    }
}

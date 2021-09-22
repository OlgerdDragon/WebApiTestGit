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
        [Route("GetNededProduct")]
        public async Task<ActionResult<IEnumerable<WantedList>>> GetShopItems() => await _husbandService.GetWantedListAsync();
        [HttpGet]
        [Route("GetShopsListForVisited")]
        public string GetShopsListForVisited() { return "ShopsListForVisited"; }
        [HttpGet]
        [Route("GetProductListForBuy")]
        public string GetProductListForBuy() { return "ProductListForBuy"; }
    }
}

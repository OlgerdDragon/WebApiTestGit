using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiGeneralGrpc.Services.HusbandService;
using HusbandGrpcService;

namespace WebApiGeneralGrpc.Controllers
{
    
    //[Authorize(Roles = "husband")]
    
    public class HusbandController : APIControllerBase
    {
        private readonly HusbandGreeter.HusbandGreeterClient _husbandServiceClient;

        public HusbandController(IHusbandServiceFactory husbandServiceFactory)
        {
            _husbandServiceClient = husbandServiceFactory.GetGrpcClient();
        }

        [HttpGet]
        public string Get()
        {
            return "Hello Husband!";
        }
        [HttpGet("Products")]
        public async Task<ActionResult<ListOfWantedProductDto>> GetNeededProductList()
        {
            var neededProductList = await _husbandServiceClient.GetWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin});
            if (!neededProductList.Successfully)
                return BadRequest(neededProductList.ErrorMessage);
            return neededProductList.Element;
        }
        [HttpGet("Shops")]
        public async Task<ActionResult<ListOfShopDto>> GetNeededShopList()
        {
            var neededShopList = await _husbandServiceClient.GetShopsForVisitAsync(new UserLoginRequest() { UserLogin = userLogin });
            if (!neededShopList.Successfully)
                return BadRequest(neededShopList.ErrorMessage);
            return neededShopList.Element;
        }
        [HttpGet("ProductsInShop/{shopId}")]
        public async Task<ActionResult<ListOfProductDto>> GetNededProductListInShop(int shopId)
        {
            var nededProductListInShop = await _husbandServiceClient.GetProductsInShopAsync(new GetProductsInShopRequest() { ShopId = shopId, UserLogin = userLogin });
            if (!nededProductListInShop.Successfully)
                return BadRequest(nededProductListInShop.ErrorMessage);
            return nededProductListInShop.Element;
        }

       
}
}

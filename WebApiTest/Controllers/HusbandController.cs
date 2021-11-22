using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models.Dto;
using WebApiTest.Services.HusbandService;
using Grpc.Net.Client;
using HusbandGrpcService;

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
        [HttpGet("ProductsM")]
        public async Task<ActionResult<ListOfWantedProductDto>> GetNeededProductListM()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var husbandService = new Greeter.GreeterClient(channel);

            var neededProductList = await husbandService.GetWantedProductsAsync(new GetWantedProductsRequest() { UserLogin = userLogin});
            if (!neededProductList.Successfully)
                return BadRequest(neededProductList.ErrorMessage);
            return neededProductList.Element;
        }
        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<WantedProductDto>>> GetNeededProductList()
        {
            var neededProductList = await _husbandService.GetWantedProductsAsync(userLogin);
            if (!neededProductList.Successfully) 
                return BadRequest(neededProductList.ExceptionMessage);
            return neededProductList.Element;
        }
        [HttpGet("ShopsM")]
        public async Task<ActionResult<ListOfShopDto>> GetNeededShopListM()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var husbandService = new Greeter.GreeterClient(channel);

            var neededShopList = await husbandService.GetShopsForVisitAsync(new GetShopsForVisitRequest() { UserLogin = userLogin });
            if (!neededShopList.Successfully)
                return BadRequest(neededShopList.ErrorMessage);
            return neededShopList.Element;
        }
        [HttpGet("Shops")]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetNeededShopList()
        {
            var neededShopList = await _husbandService.GetShopsForVisitAsync(userLogin);
            if (!neededShopList.Successfully) 
                return BadRequest(neededShopList.ExceptionMessage);
            return neededShopList.Element;
        }
        [HttpGet("ProductsInShopM/{shopId}")]
        public async Task<ActionResult<ListOfProductDto>> GetNededProductListInShopM(int shopId)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var husbandService = new Greeter.GreeterClient(channel);

            var nededProductListInShop = await husbandService.GetProductsInShopAsync(new GetProductsInShopRequest() { ShopId = shopId, UserLogin = userLogin });
            if (!nededProductListInShop.Successfully)
                return BadRequest(nededProductListInShop.ErrorMessage);
            return nededProductListInShop.Element;
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

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Controllers
{
    public class HusbandController : Controller
    {
        [HttpGet]
        [Route("GetNededProduct")]
        public string GetNededProduct() { return "NededProduct"; }
        [HttpGet]
        [Route("GetShopsListForVisited")]
        public string GetShopsListForVisited() { return "ShopsListForVisited"; }
        [HttpGet]
        [Route("GetProductListForBuy")]
        public string GetProductListForBuy() { return "ProductListForBuy"; }
    }
}

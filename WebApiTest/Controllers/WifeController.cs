using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Controllers
{
    public class WifeController : Controller
    {
        [HttpPost]
        [Route("CreatedProductList")]
        public string CreatedProductList() 
        {
            
            return "I done CreatedProductList"; 
        }
        [HttpPost]
        [Route("AddProduct")]
        public string AddProduct() { return "I done AddProduct"; }
        [HttpDelete]
        [Route("DeletedProduct")]
        public string GetNededProduct() { return "I done GetNededProduct"; }
       
        
        
    }
}

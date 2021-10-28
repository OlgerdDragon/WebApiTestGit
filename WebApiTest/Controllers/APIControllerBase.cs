using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiTest.Data;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public abstract class APIControllerBase : Controller
    {
        public readonly string userLogin;
        public APIControllerBase()
        {
            userLogin = new HttpContextAccessor().HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        }
    }
}

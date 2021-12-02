using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WebApiGeneralGrpc.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public abstract class APIControllerBase : Controller
    {
        public readonly string userLogin;

        public APIControllerBase()
        {
            //userLogin = new HttpContextAccessor().HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            userLogin = "";
        }
    }
}

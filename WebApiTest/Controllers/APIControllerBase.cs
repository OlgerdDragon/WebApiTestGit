using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class APIControllerBase : Controller
    {

    }
}

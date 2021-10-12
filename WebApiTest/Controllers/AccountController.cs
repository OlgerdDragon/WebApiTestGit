using Microsoft.AspNetCore.Mvc;
using WebApiTest.Services.AccountService;

namespace WebApiTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("/Token")]
        public IActionResult Token(string username, string password)
        {
            var identity = _accountService?.Token(username, password);
            if (identity == null)
                    return BadRequest(new { errorText = "Invalid username or password." });

            return Json(identity);
        }
    }
}
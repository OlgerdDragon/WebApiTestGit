using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiMonolit.Models.Dto;
using WebApiMonolit.Services.AccountService;

namespace WebApiMonolit.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Token")]
        public async Task<IActionResult> Token(EntryAccountDto account)
        {            
            var result = await _accountService?.Token(account.Username, account.Password);
            if (!result.Successfully) return BadRequest(result.ExceptionMessage);

            var identity = result.Element;
            if (identity == null)                  
                return BadRequest(new { errorText = "Invalid username or password." });    
            
            return Json(identity);           
            
        }
    }
}
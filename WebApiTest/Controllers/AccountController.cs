using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiTest.Models.Dto;
using WebApiTest.Services.AccountService;

namespace WebApiTest.Controllers
{
    public class AccountController : APIControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Token")]
        public async Task<ActionResult<EntryAccountDto>> Token(EntryAccountDto account)
        {            
            var result = await _accountService?.Token(account.Username, account.Password);
            if (!result.Successfully) return BadRequest();

            var identity = result.Element;
            if (identity == null)                  
                return BadRequest(new { errorText = "Invalid username or password." });    
            
            return Json(identity);           
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiTest.Models; // класс Person
using WebApiTest;
using WebApiTest.Data;
using WebApiTest.Services.ValuesService;

namespace WebApiTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly IValuesService _valuesService;
        public AccountController(IValuesService valuesService)
        {
            _valuesService = valuesService;
        }

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = _valuesService.GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        
    }
}
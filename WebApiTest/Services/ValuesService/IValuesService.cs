using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApiTest.Services.ValuesService
{
    public interface IValuesService
    {
        public ClaimsIdentity GetIdentity(string username, string password);
    }
}

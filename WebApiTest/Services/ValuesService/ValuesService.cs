using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WebApiTest.Data;
using WebApiTest.Models;

namespace WebApiTest.Services.ValuesService
{
    public class ValuesService : IValuesService
    {
        private readonly TowerContext _context;
        public ValuesService(TowerContext context)
        {
            _context = context;
        }
        public ClaimsIdentity GetIdentity(string username, string password)
        {
            Person person = _context.Persons
                .Where(x => x.Login == username && x.Password == password)
                .FirstOrDefault();
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}

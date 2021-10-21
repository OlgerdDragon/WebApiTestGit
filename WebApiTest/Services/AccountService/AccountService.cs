using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiTest.Data;
using WebApiTest.Models;

namespace WebApiTest.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly TownContext _context;

        public AccountService(TownContext context)
        {
            _context = context;
        }

        public object Token(string username, string password)
        {
            try
            {
                var identity = GetIdentity(username, password);
                if (identity == null)
                {
                    return null;
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

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Token method:" + ex.Message);
            }
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            try
            {
                Person person = _context.Persons
                .Where(x => x.Login == username)
                .FirstOrDefault();
                bool access = GetHash(person, password);

                if (person != null && access)
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
            catch (Exception ex)
            {
                throw new Exception("GetIdentity method:" + ex.Message);
            }
        }

        private Boolean GetHash(Person person, string password)
        {
            try
            {
                byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(password);
                byte[] tmpChekHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
                var chekHash = ByteArrayToString(tmpChekHash);

                if (chekHash == person.Password)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("GetHash method:" + ex.Message);
            }
            
        }

        private string ByteArrayToString(byte[] arrInput)
        {
            try
            {
                int i;
                StringBuilder sOutput = new StringBuilder(arrInput.Length);
                for (i = 0; i < arrInput.Length - 1; i++)
                {
                    sOutput.Append(arrInput[i].ToString("X2"));
                }
                return sOutput.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("ByteArrayToString method:" + ex.Message);
            }
        }
    }
}

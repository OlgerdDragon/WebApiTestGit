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
using WebApiGeneral.Data;
using Microsoft.Extensions.Logging;
using TownContextForWebService;
using TownContextForWebService.Models;

namespace WebApiGeneral.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly TownContext _context;
        private readonly ILogger<AccountService> _logger;


        public AccountService(TownContext context, ILogger<AccountService> logger)
        {
            _context = context;
            _logger = logger;
            
        }
        
        public async Task<Result<object>> Token(string username, string password)
        {
            try
            {
                _logger.LogDebug($"Token - username: {username} password: {password}");
                var result = GetIdentity(username, password);
                _logger.LogDebug($"result.Successfully: {result.Successfully} result.Element: {result.Element}");


                if (result.Element == null)
                {
                    if (!result.Successfully)
                    {
                        _logger.LogDebug($"Token error: {result.ExceptionMessage}");
                        return new Result<object>(result.ExceptionMessage);
                    }
                    _logger.LogDebug($"Token null: {result.Element}");
                    return new Result<object>(null);
                }
                

                var identity = result.Element;
                var now = DateTime.UtcNow;
                _logger.LogDebug($"DateTime now: {now}");

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

                _logger.LogInformation($"Token return - username: {response.username} access_token: {response.access_token} ");
                return new Result<object>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token");
                return new Result<object>(ex);
            }
        }

        private Result<ClaimsIdentity> GetIdentity(string username, string password)
        {
            try
            {
                _logger.LogDebug($"GetIdentity - username: {username} password: {password}");

                Person person = _context.Persons
                .Where(x => x.Login == username)
                .FirstOrDefault();
                _logger.LogDebug($"person.Login: {person.Login} person.Role: {person.Role}");

                var result = GetHash(person, password);
                bool access = result.Element;
                _logger.LogDebug($"result.Successfully: {result.Successfully} result.Element: {result.Element}");

                ClaimsIdentity claimsIdentity=null;
                if (person != null && access)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                    claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                }

                _logger.LogDebug($"GetIdentity return - claimsIdentity.Name: {claimsIdentity.Name}");
                return new Result<ClaimsIdentity>(claimsIdentity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetIdentity");
                return new Result<ClaimsIdentity>(ex);
            }
        }

        private Result<Boolean> GetHash(Person person, string password)
        {
            try
            {
                _logger.LogDebug($"GetHash - person.Login: {person.Login} person.Password: {person.Password} person.Role: {person.Role} password: {password}");

                byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(password);
                _logger.LogDebug($"tmpSource: {tmpSource}");

                byte[] tmpChekHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
                _logger.LogDebug($"tmpChekHash: {tmpChekHash}");

                var result = ByteArrayToString(tmpChekHash);
                _logger.LogDebug($"result.Successfullyn: {result.Successfully} result.Element: {result.Element}");

                var chekHash = result.Element;
                bool same = false;
                if (chekHash == person.Password)
                    same = true;

                _logger.LogDebug($"GetHash return - same: {same}");
                return new Result<Boolean>(same);
            }
            catch (Exception ex)
            {
                var res = new Result<Boolean>(ex);
                return res;
            }
            
        }

        private Result<string> ByteArrayToString(byte[] arrInput)
        {
            try
            {
                StringBuilder sOutput = new StringBuilder(arrInput.Length);
                for (int i = 0; i < arrInput.Length - 1; i++)
                {
                    sOutput.Append(arrInput[i].ToString("X2"));
                }
                return new Result<string>(sOutput.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("ByteArrayToString method:" + ex.Message);
            }
        }
    }
}

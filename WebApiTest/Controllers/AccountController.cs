﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiTest.Models; // класс Person
using WebApiTest;
using WebApiTest.Data;

namespace WebApiTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly TowerContext _context;

        public AccountController(TowerContext context)
        {
            _context = context;
        }
        // тестовые данные вместо использования базы данных
        private List<Person> people = new List<Person>
        {
            new Person {Login="admin@gmail.com", Password="1", Role = "admin" },
            new Person {Login="husband@gmail.com", Password="1", Role = "husband" },
            new Person {Login="wife@gmail.com", Password="1", Role = "wife" },
            new Person { Login="qwerty@gmail.com", Password="5", Role = "user" }
        };

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        //public IActionResult Token(Person person)
        {
            //string username = person.Login;
            //string password = person.Password;
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
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

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            //Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
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

            // если пользователя не найдено
            return null;
        }
    }
}
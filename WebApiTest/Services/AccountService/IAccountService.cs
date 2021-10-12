using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Services.AccountService
{
    public interface IAccountService
    {
        public object Token(string username, string password);
    }
}

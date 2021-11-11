using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WebApiTest.Data;
using WebApiTest.Services.AccountService;
using WebApiTest.Services.AdminService;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Models;
using WebAPITest.XUnitTests.Extensions;
using WebAPITest.XUnitTests.Infra;

namespace WebAPITest.XUnitTests.Common
{
    public class AccountServiceTests
    {
        private IAccountService _accountService;
        private readonly Mock<DbContextOptions<TownContext>> _optionsTown = new Mock<DbContextOptions<TownContext>>();
        private readonly Mock<TownContext> _context = new Mock<TownContext>(new DbContextOptions<TownContext>());
        private readonly Mock<ILogger<AccountService>> _logger = new Mock<ILogger<AccountService>>();

        public AccountServiceTests()
        {
            _accountService = new AccountService(_context.Object, _logger.Object);
        }

        [Fact]
        public async Task GetTokenAsync_ShouldReturnNULL_WhenCustomerExists()
        {
            //Arrange
            
            //Act
            var result = await _accountService?.Token("", "");

            //Assert
            Assert.Null(result.Element);
        }

        [Fact]
        public async Task GetTokenAsync_ShouldReturnNULLExeption_WhenNULLobjects()
        {
            //Arrange

            //Act
            var result = await _accountService.Token(null,null);

            //Assert
            Assert.NotNull(result.ExceptionMessage);
        }
        [Fact]
        public async Task GetTokenAsync_ShouldReturnUsernameAndJwt_WhenUsernameAndPasword()
        {
            //Arrange
            var username = "admin@gmail.com";
            var password = "1";

            var data = new List<Person>
            {
                new()
                {
                    Login = "admin@gmail.com",
                    Password = "1",
                    Role = "admin"
                }
            };

            _context.Setup(c => c.Persons).Returns(data.BuildMockDbSet());

            //Act

            var blogs = await _accountService.Token(username, password);

            //Assert
            Assert.NotNull(blogs.Element);
        }


    }
}

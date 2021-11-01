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
using Microsoft.EntityFrameworkCore;

namespace WebAPITest.XUnitTests.Common
{
    public class AccountServiceTests
    {
        private readonly IAccountService _accountService;
        private readonly Mock<DbContextOptions<TownContext>> _optionsTown = new Mock<DbContextOptions<TownContext>>();
        private readonly Mock<TownContext> _context;
        private readonly Mock<ILogger<AccountService>> _logger = new Mock<ILogger<AccountService>>();
        public AccountServiceTests()
        {
            _context = new Mock<TownContext>(_optionsTown.Object);
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
    }
}

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

namespace WebAPITest.XUnitTests.Common
{
    public class AccountServiceTests
    {
        private readonly IAccountService _accountService;
        private IAccountService _accountService1;
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
            var result = await _accountService?.Token(null,null);

            //Assert
            Assert.False(result.Successfully);
        }
        [Fact]
        public async Task GetTokenAsync_ShouldReturnUsernameAndJwt_WhenUsernameAndPasword()
        {
            //Arrange
            var username = "admin@gmail.com";
            var password = "1";
            Person person = new Person { Login = "admin@gmail.com", Password = "1" };
            _context.Object.Persons.Add(person);

            //Act
            var result = await _accountService?.Token(username, password);

            //Assert
            Assert.NotNull(result.Element);
        }
        [Fact]
        public async Task GetTokenAsync_ShouldReturnUsernameAndJwt_v2_WhenUsernameAndPasword()
        {
            //Arrange
            var username = "admin@gmail.com";
            var password = "1";

            var data = new List<Person>
            {
                new Person { Login = "admin@gmail.com", Password = "1", Role = "admin" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Person>>();
            mockSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<TownContext>(new DbContextOptions<TownContext>());
            mockContext.Setup(c => c.Persons).Returns(mockSet.Object);

            var service = new AccountService(mockContext.Object, _logger.Object);

            //Act
            
            var blogs = await service.Token(username, password);

            //Assert
            Assert.NotNull(blogs.Element);

            Assert.True(true);
        }
       
        public async Task GetTokenAsync_NewMock()
        {
            //Arrange
            var data = new List<Shop>
            {
                new Shop { Id=1, Name = "Metro" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Shop>>();
            mockSet.As<IQueryable<Shop>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Shop>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Shop>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Shop>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<TownContext>(new DbContextOptions<TownContext>());
            mockContext.Setup(c => c.Shops).Returns(mockSet.Object);

            Mock<ILogger<AdminService>> _loggerAdmin = new Mock<ILogger<AdminService>>();
            var service = new AdminService(mockContext.Object, _loggerAdmin.Object);

            //Act

            var blogs = await service.GetShopsAsync();

            //Assert
            Assert.NotNull(blogs.Element);

            Assert.True(true);
        }
    }
}

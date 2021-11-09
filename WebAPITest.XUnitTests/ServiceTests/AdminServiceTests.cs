using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WebApiTest.Data;
using WebApiTest.Services.AccountService;
using WebApiTest.Services.AdminService;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Models;
using WebApiTest.Controllers;
using WebApiTest.Models.Dto;
using WebAPITest.XUnitTests.Extensions;
using WebAPITest.XUnitTests.Infra;


namespace WebAPITest.XUnitTests.Common
{
    public class AdminServiceTests
    {
        private AdminController _adminController;

        private IAdminService _adminService;
        private Mock<DbContextOptions<TownContext>> _optionsTown = new Mock<DbContextOptions<TownContext>>();
        private Mock<TownContext> _context = new Mock<TownContext>(new DbContextOptions<TownContext>());
        private Mock<ILogger<AdminService>> _logger = new Mock<ILogger<AdminService>>();

        public AdminServiceTests()
        {
            

        }
        [Fact]
        public async Task GetProductsAsync_ShouldReturnOneProduct_WhenOneProduct()
        {
            //Arrange
            var data = new List<Product>
            {
                new() 
                { 
                    Id=1, 
                    Name = "Salo", 
                    Price = 100, 
                    ShopId = 1 
                }
            };

            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());

            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsAsync();

            //Assert
            var some = false;
            if (realData.Element.Count == 1
                    && realData.Element[0].Id == 1
                    && realData.Element[0].Name == "Salo"
                    && realData.Element[0].Price == 100
                    && realData.Element[0].ShopId == 1) some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetProductsAsync_ShouldReturnZero_WhenZeroProduct()
        {
            //Arrange
            var data = new List<Product>();

            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());

            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsAsync();

            //Assert
            var some = false;
            if (realData.Element.Count == 0)
                some = true;

            Assert.True(some);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnNullException_WhenNotHaveProducts()
        {
            //Arrange


            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsAsync();

            //Assert

            Assert.NotNull(realData.ExceptionMessage);
        }
        [Fact]
        public async Task GetShopsAsync_ShouldReturnOneShop_WhenOneShop()
        {
            //Arrange
            var data = new List<Shop>
            {
                new() 
                { 
                    Id=1, 
                    Name = "Metro" 
                }
            };

            _context.Setup(p => p.Shops).Returns(data.BuildMockDbSet());
            
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsAsync();
            
            //Assert
            var some = false;
            if (realData.Element.Count == 1)
            {
                if (realData.Element[0].Id == 1 && realData.Element[0].Name == "Metro") some = true;
            }
            Assert.True(some);
        }
        [Fact]
        public async Task GetShopsAsync_ShouldReturnZero_WhenZeroShop()
        {
            //Arrange
            var data = new List<Shop>();

            _context.Setup(p => p.Shops).Returns(data.BuildMockDbSet());

            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsAsync();

            //Assert
            var some = false;
            if (realData.Element.Count == 0)
                some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetShopsAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange


            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsAsync();

            //Assert

            Assert.NotNull(realData.ExceptionMessage);
        }
        
        
    }
}

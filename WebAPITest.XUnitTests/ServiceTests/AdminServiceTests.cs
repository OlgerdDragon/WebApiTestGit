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
        public async Task Test_Mock()
        {
            //Arrange
            var data = new List<Shop>
            {
                new() { Id=1, Name = "Metro" }
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

        
    }
}

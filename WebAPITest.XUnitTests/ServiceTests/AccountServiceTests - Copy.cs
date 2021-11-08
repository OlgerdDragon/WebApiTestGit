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
using WebApiTest.Controllers;
using WebApiTest.Models.Dto;


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
                new Shop { Id=1, Name = "Metro" }
            }.AsQueryable();
            var shopSet = new Mock<DbSet<Shop>>();
            //var list = new Mock<IEnumerable<Shop>>(MockBehavior.Strict);

            //list.As<IEnumerable<Shop>>().Setup(x => x.id)

            //shopSet.As<IEnumerable<Shop>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            shopSet.As<IQueryable<Shop>>().Setup(m => m.Provider).Returns(data.Provider);
            shopSet.As<IQueryable<Shop>>().Setup(m => m.Expression).Returns(data.Expression);
            shopSet.As<IQueryable<Shop>>().Setup(m => m.ElementType).Returns(data.ElementType);
            shopSet.As<IQueryable<Shop>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            

            _context.Setup(p => p.Shops).Returns(shopSet.Object);

            //shopSet
            //.Setup(x => x.Select(i => new ShopDto
            //{
            //    Id = i.Id,
            //    Name = i.Name
            //})).Returns(data);

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

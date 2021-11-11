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
using Microsoft.EntityFrameworkCore;
using WebApiTest.Models;
using WebApiTest.Controllers;
using WebApiTest.Models.Dto;
using WebAPITest.XUnitTests.Extensions;
using WebAPITest.XUnitTests.Infra;
using WebApiTest.Services.WifeService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest.XUnitTests.Common
{
    public class WifeServiceTests
    {
        private AdminController _adminController;

        private IWifeService _wifeService;
        private Mock<DbContextOptions<TownContext>> _optionsTown = new Mock<DbContextOptions<TownContext>>();
        private Mock<TownContext> _context = new Mock<TownContext>(new DbContextOptions<TownContext>());
        private Mock<ILogger<WifeService>> _logger = new Mock<ILogger<WifeService>>();

        private string userLogin = "husbandUnitTest";
        public WifeServiceTests()
        {

        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnOneWantedProduct_WhenHaveOneWantedProducts()
        {
            //Arrange
            var data = new List<WantedProduct>
            {
                new()
                {
                    Id = 1,
                    BoughtStatus = false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetWantedProductsAsync();

            //Assert
            var some = false;
            if (realData.Element.Count == 1
                    && realData.Element[0].Id == 1
                    && realData.Element[0].BoughtStatus == false
                    && realData.Element[0].ProductId == 1
                    && realData.Element[0].WifeId == 1) some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnZeroList_WhenHaveZeroWantedProducts()
        {
            //Arrange
            var data = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetWantedProductsAsync();

            //Assert
            var some = false;
            if (realData.Element.Count == 0)
                some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetWantedProductsAsync();

            //Assert

            Assert.NotNull(realData.ExceptionMessage);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturn100_WhenHaveOneWantedProducts()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>
            {
                new()
                {
                    Id = 1,
                    BoughtStatus = false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            var product = new Product
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };

            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.Products.FindAsync(dataWantedProduct[0].ProductId)).Returns(new ValueTask<Product>(product));

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetTotalAmountWantedProductsAsync();

            //Assert
            int expected = 100;
            Assert.Equal(expected, realData.Element);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturn0_WhenHaveZeroWantedProducts()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>();
            var dataProduct = new List<Product>();

            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetTotalAmountWantedProductsAsync();

            //Assert
            int expected = 0;
            Assert.Equal(expected, realData.Element);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetTotalAmountWantedProductsAsync();

            //Assert

            Assert.NotNull(realData.ExceptionMessage);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnTrue_WhenOneWantedProducts()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>
            {
                new()
                {
                    Id = 1,
                    BoughtStatus = false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            var wantedproduct = new WantedProduct
            {
                Id = 1,
                BoughtStatus = false,
                ProductId = 1,
                WifeId = 1
            };
            var wantedProductId = 1;
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.WantedProducts.FindAsync(wantedProductId)).Returns(new ValueTask<WantedProduct>(wantedproduct));
            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.RemoveWantedProduct(wantedProductId, userLogin);

            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnFalse_WhenZeroWantedProducts()
        {
            //Arrange
            var data = new List<WantedProduct>();
            var wantedProductId = 1;
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.RemoveWantedProduct(wantedProductId, userLogin);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            var wantedProductId = 1;
            
            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.RemoveWantedProduct(wantedProductId, userLogin);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnTrue_WhenOneWantedProduct()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>
            {
                new()
                {
                    Id = 1,
                    BoughtStatus = false,
                    ProductId = 1,
                    WifeId = 1
                }
            };

            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.RemoveAllWantedProducts(userLogin);

            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnTrue_WhenZeroWantedProduct()
        {
            //Arrange
            var data = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.RemoveAllWantedProducts(userLogin);

            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.RemoveAllWantedProducts(userLogin);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnTrue_WhenOneWantedProducts()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>
            {
                new()
                {
                    Id = 1,
                    BoughtStatus = false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            var wantedproduct = new WantedProduct
            {
                Id = 1,
                BoughtStatus = false,
                ProductId = 1,
                WifeId = 1
            };

            var wantedProductId = 1;
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.WantedProducts.FindAsync(wantedProductId)).Returns(new ValueTask<WantedProduct>(wantedproduct));
            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetWantedProductItemAsync(wantedProductId);

            //Assert
            var actual = realData.Element.Value;
            var some = false;
            if (actual.Id == 1
                    && actual.BoughtStatus == false
                    && actual.ProductId == 1
                    && actual.WifeId == 1) some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnNULL_WhenZeroWantedProducts()
        {
            //Arrange
            var data = new List<WantedProduct>();
            var wantedProductId = 1;
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetWantedProductItemAsync(wantedProductId);

            //Assert
            Assert.Null(realData.Element);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            var wantedProductId = 1;

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.GetWantedProductItemAsync(wantedProductId);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnTrue_WhenAddOneWantedProducts()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>();
            var product = new Product
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            var wantedProductId = 1;
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.Products.FindAsync(wantedProductId)).Returns(new ValueTask<Product>(product));
            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.AddProduct(wantedProductId, userLogin);

            //Assert
            var some = false;
            if (realData.Element.ProductId == 1) some = true;
            Assert.True(some);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNULL_WhenZeroProducts()
        {
            //Arrange
            var wantedProductId = 1;
            var dataWantedProduct = new List<WantedProduct>();
            var dataProduct = new List<Product>();

            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.AddProduct(wantedProductId, userLogin);

            //Assert
            var expected = 0;
            Assert.Equal(expected, realData.Element.Id);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNullException_WhenNotHaveProducts()
        {
            //Arrange
            var wantedProductId = 1;

            //Act
            _wifeService = new WifeService(_context.Object, _logger.Object);
            var realData = await _wifeService.AddProduct(wantedProductId, userLogin);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
        }
    }
}

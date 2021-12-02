using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WebApiMonolit.Data;
using Microsoft.EntityFrameworkCore;
using WebApiMonolit.Models;
using WebApiMonolit.XUnitTests.Extensions;
using WebApiMonolit.Services.HusbandService;

namespace WebApiMonolit.XUnitTests.Common
{
    public class HusbandServiceTests
    {
        private IHusbandService _husbandService;
        private Mock<DbContextOptions<TownContext>> _optionsTown = new Mock<DbContextOptions<TownContext>>();
        private Mock<TownContext> _context = new Mock<TownContext>(new DbContextOptions<TownContext>());
        private Mock<ILogger<HusbandService>> _logger = new Mock<ILogger<HusbandService>>();

        private string userLogin = "husbandUnitTest";


        public HusbandServiceTests()
        {

        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
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
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetWantedProductsAsync(userLogin);

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
        public async Task GetWantedProductsAsync_ShouldReturnZero_WhenZeroProduct()
        {
            //Arrange
            var data = new List<WantedProduct>();

            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetWantedProductsAsync(userLogin);

            //Assert
            var some = false;
            if (realData.Element.Count == 0)
                some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange

            //Act
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetWantedProductsAsync(userLogin);

            //Assert

            Assert.NotNull(realData.ExceptionMessage);
        }
        [Fact]
        public async Task GetShopsForVisitAsync_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
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
            var dataProduct = new List<Product>
            {
                new()
                {
                    Id = 1,
                    Name = "Salo",
                    Price = 100,
                    ShopId = 1
                }
            };
            var shop = new Shop
            {
                Id = 1,
                Name = "Metro"
            };
            
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            _context.Setup(p => p.Products.FindAsync(dataWantedProduct[0].ProductId)).Returns(new ValueTask<Product>(dataProduct[0]));
            _context.Setup(p => p.Shops.FindAsync(dataProduct[0].ShopId)).Returns(new ValueTask<Shop>(shop));
            //Act
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetShopsForVisitAsync(userLogin);

            //Assert
            var some = false;
            if (realData.Element.Count == 1
                    && realData.Element[0].Id == 1
                    && realData.Element[0].Name == "Metro") some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetShopsForVisitAsync_ShouldReturnZero_WhenZeroProduct()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>();
            var dataShop = new List<Shop>();
            var dataProduct = new List<Product>();

            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());

            //Act
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetShopsForVisitAsync(userLogin);

            //Assert
            var some = false;
            if (realData.Element.Count == 0)
                some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetShopsForVisitAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange


            //Act
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetShopsForVisitAsync(userLogin);

            //Assert

            Assert.NotNull(realData.ExceptionMessage);
        }

        [Fact]
        public async Task GetProductsInShopAsync_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
        {
            //Arrange
            var shopId = 1;
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
            var dataProduct = new List<Product>
            {
                new()
                {
                    Id=1,
                    Name = "Salo",
                    Price = 100,
                    ShopId = 1
                }
            };

            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            _context.Setup(p => p.Products.FindAsync(dataWantedProduct[0].ProductId)).Returns(new ValueTask<Product>(dataProduct[0]));
            //Act
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetProductsInShopAsync(shopId, userLogin);

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
        public async Task GetProductsInShopAsync_ShouldReturnZero_WhenZeroProduct()
        {
            //Arrange
            var shopId = 1;
            var dataWantedProduct = new List<WantedProduct>();
            var dataProduct = new List<Product>();

            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());

            //Act
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetProductsInShopAsync(shopId, userLogin);

            //Assert
            var some = false;
            if (realData.Element.Count == 0)
                some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetProductsInShopAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopId = 1;

            //Act
            _husbandService = new HusbandService(_context.Object, _logger.Object);
            var realData = await _husbandService.GetProductsInShopAsync(shopId, userLogin);

            //Assert

            Assert.NotNull(realData.ExceptionMessage);
        }
    }
}

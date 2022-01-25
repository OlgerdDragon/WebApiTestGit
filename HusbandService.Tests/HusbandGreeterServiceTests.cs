using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HusbandService.Services;
using HusbandService.Tests.Extensions;
using Grpc.Core;
using DbApiContextForService;
using DbApiContextForService.Models;

namespace HusbandService.Tests
{
    public class HusbandGreeterServiceTests
    {
        private HusbandGreeterService _husbandService;
        private Mock<DbContextOptions<DbApiContext>> _optionsTown = new Mock<DbContextOptions<DbApiContext>>();
        private Mock<DbApiContext> _context = new Mock<DbApiContext>(new DbContextOptions<DbApiContext>());
        private Mock<ServerCallContext> serverCallContext = new Mock<ServerCallContext>();
        private Mock<ILogger<HusbandGreeterService>> _logger = new Mock<ILogger<HusbandGreeterService>>();

        private string userLogin = "husbandUnitTest";


        public HusbandGreeterServiceTests()
        {

        }
        //    [Fact]
        //    public async Task GetWantedProductsAsync_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
        //    {
        //        //Arrange
        //        var data = new List<WantedProduct>
        //        {
        //            new()
        //            {
        //                Id = 1,
        //                BoughtStatus = false,
        //                ProductId = 1,
        //                WifeId = 1
        //            }
        //        };

        //        _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

        //        //Act
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await _husbandService.GetWantedProducts(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

        //        //Assert
        //        var some = false;
        //        if (realData.Element.WantedProductDtoMessage.Count == 1
        //                && realData.Element.WantedProductDtoMessage[0].Id == 1
        //                && realData.Element.WantedProductDtoMessage[0].BoughtStatus == false
        //                && realData.Element.WantedProductDtoMessage[0].ProductId == 1
        //                && realData.Element.WantedProductDtoMessage[0].WifeId == 1) some = true;

        //        Assert.True(some);
        //    }
        //    [Fact]
        //    public async Task GetWantedProductsAsync_ShouldReturnZero_WhenZeroProduct()
        //    {
        //        //Arrange
        //        var data = new List<WantedProduct>();

        //        _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

        //        //Act
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await  _husbandService.GetWantedProducts(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

        //        //Assert
        //        var some = false;
        //        if (realData.Element.WantedProductDtoMessage.Count == 0)
        //            some = true;

        //        Assert.True(some);
        //    }
        //    [Fact]
        //    public async Task GetWantedProductsAsync_ShouldReturnNullException_WhenNotHaveShops()
        //    {
        //        //Arrange

        //        //Act
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await  _husbandService.GetWantedProducts(new UserLoginRequest { UserLogin = userLogin}, serverCallContext.Object);

        //        //Assert

        //        Assert.NotNull(realData.ErrorMessage);
        //    }
        //    [Fact]
        //    public async Task GetShopsForVisitAsync_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
        //    {
        //        //Arrange
        //        var dataWantedProduct = new List<WantedProduct>
        //        {
        //            new()
        //            {
        //                Id = 1,
        //                BoughtStatus = false,
        //                ProductId = 1,
        //                WifeId = 1
        //            }
        //        };
        //        var dataProduct = new List<Product>
        //        {
        //            new()
        //            {
        //                Id = 1,
        //                Name = "Salo",
        //                Price = 100,
        //                ShopId = 1
        //            }
        //        };
        //        var shop = new Shop
        //        {
        //            Id = 1,
        //            Name = "Metro"
        //        };

        //        _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
        //        _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
        //        _context.Setup(p => p.Products.FindAsync(dataWantedProduct[0].ProductId)).Returns(new ValueTask<Product>(dataProduct[0]));
        //        _context.Setup(p => p.Shops.FindAsync(dataProduct[0].ShopId)).Returns(new ValueTask<Shop>(shop));
        //        //Act
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await  _husbandService.GetShopsForVisit(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

        //        //Assert
        //        var some = false;
        //        if (realData.Element.ShopDtoMessage.Count == 1
        //                && realData.Element.ShopDtoMessage[0].Id == 1
        //                && realData.Element.ShopDtoMessage[0].Name == "Metro") some = true;

        //        Assert.True(some);
        //    }
        //    [Fact]
        //    public async Task GetShopsForVisitAsync_ShouldReturnZero_WhenZeroProduct()
        //    {
        //        //Arrange
        //        var dataWantedProduct = new List<WantedProduct>();
        //        var dataShop = new List<Shop>();
        //        var dataProduct = new List<Product>();

        //        _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
        //        _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
        //        _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());

        //        //Act
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await  _husbandService.GetShopsForVisit(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

        //        //Assert
        //        var some = false;
        //        if (realData.Element.ShopDtoMessage.Count == 0)
        //            some = true;

        //        Assert.True(some);
        //    }
        //    [Fact]
        //    public async Task GetShopsForVisitAsync_ShouldReturnNullException_WhenNotHaveShops()
        //    {
        //        //Arrange


        //        //Act
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await  _husbandService.GetShopsForVisit(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

        //        //Assert

        //        Assert.NotNull(realData.ErrorMessage);
        //    }

        //    [Fact]
        //    public async Task GetProductsInShopAsync_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
        //    {
        //        //Arrange
        //        var shopId = 1;
        //        var dataWantedProduct = new List<WantedProduct>
        //        {
        //            new()
        //            {
        //                Id = 1,
        //                BoughtStatus = false,
        //                ProductId = 1,
        //                WifeId = 1
        //            }
        //        };
        //        var dataProduct = new List<Product>
        //        {
        //            new()
        //            {
        //                Id=1,
        //                Name = "Salo",
        //                Price = 100,
        //                ShopId = 1
        //            }
        //        };

        //        _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
        //        _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
        //        _context.Setup(p => p.Products.FindAsync(dataWantedProduct[0].ProductId)).Returns(new ValueTask<Product>(dataProduct[0]));
        //        //Act
        //        var itemRequest = new GetProductsInShopRequest { ShopId = shopId, UserLogin = userLogin };
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await  _husbandService.GetProductsInShop(itemRequest, serverCallContext.Object);

        //        //Assert
        //        var some = false;
        //        if (realData.Element.ProductDtoMessage.Count == 1
        //                && realData.Element.ProductDtoMessage[0].Id == 1
        //                && realData.Element.ProductDtoMessage[0].Name == "Salo"
        //                && realData.Element.ProductDtoMessage[0].Price == 100
        //                && realData.Element.ProductDtoMessage[0].ShopId == 1) some = true;

        //        Assert.True(some);
        //    }
        //    [Fact]
        //    public async Task GetProductsInShopAsync_ShouldReturnZero_WhenZeroProduct()
        //    {
        //        //Arrange
        //        var shopId = 1;
        //        var dataWantedProduct = new List<WantedProduct>();
        //        var dataProduct = new List<Product>();

        //        _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
        //        _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());

        //        //Act
        //        var itemRequest = new GetProductsInShopRequest { ShopId = shopId, UserLogin = userLogin };
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await  _husbandService.GetProductsInShop(itemRequest, serverCallContext.Object);

        //        //Assert
        //        var some = false;
        //        if (realData.Element.ProductDtoMessage.Count == 0)
        //            some = true;

        //        Assert.True(some);
        //    }
        //    [Fact]
        //    public async Task GetProductsInShopAsync_ShouldReturnNullException_WhenNotHaveShops()
        //    {
        //        //Arrange
        //        var shopId = 1;

        //        //Act
        //        var itemRequest = new GetProductsInShopRequest { ShopId = shopId, UserLogin = userLogin };
        //        _husbandService = new HusbandGreeterService(_context.Object, _logger.Object);
        //        var realData = await  _husbandService.GetProductsInShop(itemRequest, serverCallContext.Object);

        //        //Assert

        //        Assert.NotNull(realData.ErrorMessage);
        //    }

    }
}

using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WifeGrpcService.Services;
using WifeGrpcService.XUnitTests.Extensions;
using TownContextForWebService;
using TownContextForWebService.Models;
using Grpc.Core;
using WifeGrpcService.Services.HusbandService;
using HusbandGrpcService;

namespace WifeGrpcService.XUnitTests
{
    //public class HusbandGreeterClientTest : HusbandGreeter.HusbandGreeterClient
    //{
    //    public override AsyncUnaryCall<HusbandGrpcService.GetWantedProductsReply> GetWantedProductsAsync(HusbandGrpcService.UserLoginRequest request, CallOptions options)
    //    {
    //        var result = new HusbandGrpcService.GetWantedProductsReply { Element = new HusbandGrpcService.ListOfWantedProductDto() };
    //        var data = new List<HusbandGrpcService.WantedProductDtoMessage>
    //            {
    //                new()
    //                {
    //                    Id = 1,
    //                    BoughtStatus = false,
    //                    ProductId = 1,
    //                    WifeId = 1
    //                }
    //            };
    //        result.Element.WantedProductDtoMessage.AddRange(data);
    //        result.Successfully = true;
    //        return result;
    //    }
    //}
    public class WifeGreeterServiceTests
    {


        private WifeGreeterService _wifeGreeterService;
        private Mock<IHusbandServiceFactory> _husbandServiceFactory = new Mock<IHusbandServiceFactory>();
        private Mock<HusbandGreeter.HusbandGreeterClient> _husbandServiceClent = new Mock<HusbandGreeter.HusbandGreeterClient>();
        private Mock<DbContextOptions<TownContext>> _optionsTown = new Mock<DbContextOptions<TownContext>>();
        private Mock<TownContext> _context = new Mock<TownContext>(new DbContextOptions<TownContext>());
        private Mock<ServerCallContext> serverCallContext = new Mock<ServerCallContext>();
        private Mock<ILogger<WifeGreeterService>> _logger = new Mock<ILogger<WifeGreeterService>>();

        private string userLogin = "husbandUnitTest";
        public WifeGreeterServiceTests()
        {

        }
        private HusbandGrpcService.GetWantedProductsReply GetWantedProductsReplyHusband()
        {
            var result = new HusbandGrpcService.GetWantedProductsReply { Element = new HusbandGrpcService.ListOfWantedProductDto() };
            var data = new List<HusbandGrpcService.WantedProductDtoMessage>
                {
                    new()
                    {
                        Id = 1,
                        BoughtStatus = false,
                        ProductId = 1,
                        WifeId = 1
                    }
                };
            result.Element.WantedProductDtoMessage.AddRange(data);
            result.Successfully = true;
            return result;
        }
        //private AsyncUnaryCall<HusbandGrpcService.GetWantedProductsReply> GetWantedProductsReplyHusbandAsync()
        //{
        //    var result = new HusbandGrpcService.GetWantedProductsReply { Element = new HusbandGrpcService.ListOfWantedProductDto() };
        //    var data = new List<HusbandGrpcService.WantedProductDtoMessage>
        //        {
        //            new()
        //            {
        //                Id = 1,
        //                BoughtStatus = false,
        //                ProductId = 1,
        //                WifeId = 1
        //            }
        //        };
        //    result.Element.WantedProductDtoMessage.AddRange(data);
        //    result.Successfully = true;
        //    return result;
        //}
        //[Fact]
        //public async Task GetWantedProductsAsync_ShouldReturnOneWantedProduct_WhenHaveOneWantedProducts()
        //{
        //    //Arrange
        //    _husbandServiceClent.Setup(p => p.GetWantedProducts(It.IsAny<HusbandGrpcService.UserLoginRequest>(), It.IsAny<CallOptions>()))
        //        .Returns(GetWantedProductsReplyHusband());

        //    var test = new AsyncUnaryCall<HusbandGrpcService.GetWantedProductsReply>();

        //    _husbandServiceClent.Setup(p => p.GetWantedProductsAsync(It.IsAny<HusbandGrpcService.UserLoginRequest>(), It.IsAny<CallOptions>()))
        //       .Returns();
        //    //_context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

        //    //Act
        //    _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
        //    _wifeGreeterService._husbandServiceClient = _husbandServiceClent.Object;
        //    var realData = await _wifeGreeterService.GetWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

        //    //Assert
        //    var some = false;
        //    if (realData.Element.WantedProductDtoMessage.Count == 1
        //            && realData.Element.WantedProductDtoMessage[0].Id == 1
        //            && realData.Element.WantedProductDtoMessage[0].BoughtStatus == false
        //            && realData.Element.WantedProductDtoMessage[0].ProductId == 1
        //            && realData.Element.WantedProductDtoMessage[0].WifeId == 1) some = true;

        //    Assert.True(some);
        //}
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnZeroList_WhenHaveZeroWantedProducts()
        {
            //Arrange
            var data = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act 
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            var some = false;
            if (realData.Element.WantedProductDtoMessage.Count == 0)
                some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange

            //Act 
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert

            Assert.NotNull(realData.ErrorMessage);
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
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

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
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            int expected = 0;
            Assert.Equal(expected, realData.Element);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange

            //Act
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert

            Assert.NotNull(realData.ErrorMessage);
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
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

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
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            var wantedProductId = 1;

            //Act
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
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
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

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
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            //Act
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
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
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            var actual = realData.Element;
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
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            var wantedProductId = 1;

            //Act 
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
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
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

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
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

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
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
    }
}

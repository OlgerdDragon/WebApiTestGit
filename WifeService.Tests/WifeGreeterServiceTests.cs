using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WifeService.Services;
using DbApiContextForService;
using Grpc.Core;
using WifeService.Services.HusbandServiceFactory;
using HusbandService;

namespace WifeService.Tests
{
    
    public class WifeGreeterServiceTests
    {


        private WifeGreeterService _wifeGreeterService;
        private Mock<IHusbandServiceFactory> _husbandServiceFactory = new Mock<IHusbandServiceFactory>();
        private Mock<HusbandGreeter.HusbandGreeterClient> _husbandServiceClient = new Mock<HusbandGreeter.HusbandGreeterClient>();
        private Mock<DbContextOptions<DbApiContext>> _optionsTown = new Mock<DbContextOptions<DbApiContext>>();
        private Mock<DbApiContext> _context = new Mock<DbApiContext>(new DbContextOptions<DbApiContext>());
        private Mock<ServerCallContext> serverCallContext = new Mock<ServerCallContext>();
        private Mock<ILogger<WifeGreeterService>> _logger = new Mock<ILogger<WifeGreeterService>>();

        private string userLogin = "husbandUnitTest";
        public WifeGreeterServiceTests()
        {

        }
                
        private HusbandService.GetWantedProductsReply GetWantedProductsReplyHusband(List<HusbandService.WantedProductDtoMessage> data)
        {
            var result = new HusbandService.GetWantedProductsReply { Element = new HusbandService.ListOfWantedProductDto() };
            result.Element.WantedProductDtoMessage.AddRange(data);
            result.Successfully = true;
            return result;
        }
        private HusbandService.GetWantedProductsReply GetWantedProductsReplyHusband()
        {
            var result = new HusbandService.GetWantedProductsReply
            {
                Successfully = false,
                ErrorMessage = new Mock<Exception>().ToString()
            };
            return result;
        }
        private HusbandService.GetTotalAmountWantedProductsReply GetTotalAmountWantedProductsReplyHusband(int data)
        {
            var result = new HusbandService.GetTotalAmountWantedProductsReply
            {
                Element = data,
                Successfully =true
            };
            return result;
        }
        private HusbandService.GetTotalAmountWantedProductsReply GetTotalAmountWantedProductsReplyHusband()
        {
            var result = new HusbandService.GetTotalAmountWantedProductsReply
            {
                Successfully = false,
                ErrorMessage = new Mock<Exception>().ToString()
            };
            return result;
        }
        private HusbandService.BoolReply BoolReplyHusband(bool data)
        {
            var result = new HusbandService.BoolReply
            {
                Element = data,
                Successfully = true
            };
            return result;
        }
        private HusbandService.BoolReply BoolReplyHusband()
        {
            var result = new HusbandService.BoolReply
            {
                Successfully = false,
                ErrorMessage = new Mock<Exception>().ToString()
            };
            return result;
        }
        private HusbandService.WantedProductReply WantedProductReplyHusband(HusbandService.WantedProductDtoMessage data)
        {
            var result = new HusbandService.WantedProductReply
            {
                Element = data,
                Successfully = true
            };
            return result;
        }
        private HusbandService.WantedProductReply WantedProductReplyHusband()
        {
            var result = new HusbandService.WantedProductReply
            {
                Successfully = false,
                ErrorMessage = new Mock<Exception>().ToString()
            };
            return result;
        }
        private AsyncUnaryCall<T> GetAsyncUnaryCall<T>(T response)
        {
            return new AsyncUnaryCall<T>(
                Task.FromResult(response),
                _ => Task.FromResult(new Metadata()), _ => new Status(), _ => new Metadata(), _ => { }, null);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnOneWantedProduct_WhenHaveOneWantedProducts()
        {
            //Arrange
            var data = new List<HusbandService.WantedProductDtoMessage>
                {
                    new()
                    {
                        Id = 1,
                        BoughtStatus = false,
                        ProductId = 1,
                        WifeId = 1
                    }
                };
            var resp = GetAsyncUnaryCall(GetWantedProductsReplyHusband(data));

            _husbandServiceClient.Setup(p =>
                    p.GetWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService = 
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetWantedProducts(new UserLoginRequest() { UserLogin = userLogin },
                serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Single(realData.Element.WantedProductDtoMessage);
            Assert.Equal(1, realData.Element.WantedProductDtoMessage[0].Id);
            Assert.False(realData.Element.WantedProductDtoMessage[0].BoughtStatus);
            Assert.Equal(1, realData.Element.WantedProductDtoMessage[0].ProductId);
            Assert.Equal(1, realData.Element.WantedProductDtoMessage[0].WifeId);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnZeroList_WhenHaveZeroWantedProducts()
        {
            //Arrange
            var data = new List<HusbandService.WantedProductDtoMessage>();
            var resp = GetAsyncUnaryCall(GetWantedProductsReplyHusband(data));

            _husbandServiceClient.Setup(p =>
                    p.GetWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Empty(realData.Element.WantedProductDtoMessage);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnException_WhenHusbandReturnException()
        {
            //Arrange
            var resp = GetAsyncUnaryCall(GetWantedProductsReplyHusband());

            _husbandServiceClient.Setup(p =>
                    p.GetWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnException_WhenHaveException()
        {
            //Arrange

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturn100_WhenHaveOneWantedProducts()
        {
            //Arrange
            var data = 100;
            var resp = GetAsyncUnaryCall(GetTotalAmountWantedProductsReplyHusband(data));
            _husbandServiceClient.Setup(p =>
                    p.GetTotalAmountWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(100, realData.Element);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturn0_WhenHaveZeroWantedProducts()
        {
            //Arrange
            var data = 0;
            var resp = GetAsyncUnaryCall(GetTotalAmountWantedProductsReplyHusband(data));
            _husbandServiceClient.Setup(p =>
                    p.GetTotalAmountWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(0, realData.Element);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturnException_WhenHusbandReturnException()
        {
            //Arrange
            var resp = GetAsyncUnaryCall(GetTotalAmountWantedProductsReplyHusband());
            _husbandServiceClient.Setup(p =>
                    p.GetTotalAmountWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);
            
            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturnException_WhenHaveException()
        {
            //Arrange

            //Act
            _wifeGreeterService = new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnTrue_WhenOneWantedProducts()
        {
            //Arrange
            var data = true;
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(BoolReplyHusband(data));
            _husbandServiceClient.Setup(p =>
                    p.RemoveWantedProductAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnFalse_WhenZeroWantedProducts()
        {
            //Arrange
            var data = false;
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(BoolReplyHusband(data));
            _husbandServiceClient.Setup(p =>
                    p.RemoveWantedProductAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnException_WheHusbandReturnException()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(BoolReplyHusband());
            _husbandServiceClient.Setup(p =>
                    p.RemoveWantedProductAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnException_WhenHaveException()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnTrue_WhenOneWantedProduct()
        {
            //Arrange
            var data = true;
            var resp = GetAsyncUnaryCall(BoolReplyHusband(data));
            _husbandServiceClient.Setup(p =>
                    p.RemoveAllWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnFalse_WhenZeroWantedProduct()
        {
            //Arrange
            var data = false;
            var resp = GetAsyncUnaryCall(BoolReplyHusband(data));
            _husbandServiceClient.Setup(p =>
                    p.RemoveAllWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnException_WhenHusbandReturnException()
        {
            //Arrange
            var resp = GetAsyncUnaryCall(BoolReplyHusband());
            _husbandServiceClient.Setup(p =>
                    p.RemoveAllWantedProductsAsync(It.IsAny<HusbandService.UserLoginRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnException_WhenHaveException()
        {
            //Arrange
            
            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnTrue_WhenOneWantedProducts()
        {
            //Arrange
            var data = new HusbandService.WantedProductDtoMessage
            {
                Id = 1,
                BoughtStatus = false,
                ProductId = 1,
                WifeId = 1
            };
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(WantedProductReplyHusband(data));

            _husbandServiceClient.Setup(p =>
                    p.GetWantedProductItemAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert            
            Assert.True(realData.Successfully);
            Assert.Equal(1, realData.Element.Id);
            Assert.False(realData.Element.BoughtStatus);
            Assert.Equal(1, realData.Element.ProductId);
            Assert.Equal(1, realData.Element.WifeId);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnNULL_WhenZeroWantedProducts()
        {
            //Arrange
            var data = new HusbandService.WantedProductDtoMessage();
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(WantedProductReplyHusband(data));

            _husbandServiceClient.Setup(p =>
                    p.GetWantedProductItemAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnException_WhenHusbandReturnException()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(WantedProductReplyHusband());
            _husbandServiceClient.Setup(p =>
                    p.GetWantedProductItemAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnException_WhenHaveException()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task AddWantedProduct_ShouldReturnTrue_WhenAddOneWantedProducts()
        {
            //Arrange
            var data = new HusbandService.WantedProductDtoMessage 
            { 
                Id = 1,
                BoughtStatus = false,
                ProductId = 1,
                WifeId = 1
            };
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(WantedProductReplyHusband(data));

            _husbandServiceClient.Setup(p =>
                    p.AddWantedProductAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(1, realData.Element.Id);
            Assert.False(realData.Element.BoughtStatus);
            Assert.Equal(1, realData.Element.ProductId);
            Assert.Equal(1, realData.Element.WifeId);
        }
        [Fact]
        public async Task AddWantedProduct_ShouldReturnNULL_WhenZeroProducts()
        {
            //Arrange
            var data = new HusbandService.WantedProductDtoMessage();
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(WantedProductReplyHusband(data));

            _husbandServiceClient.Setup(p =>
                    p.AddWantedProductAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task AddWantedProduct_ShouldReturnException_WhenHusbandReturnException()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(WantedProductReplyHusband());

            _husbandServiceClient.Setup(p =>
                    p.AddWantedProductAsync(It.IsAny<HusbandService.ItemRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            _wifeGreeterService._husbandServiceClient = _husbandServiceClient.Object;
            var realData = await _wifeGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task AddWantedProduct_ShouldReturnException_WhenHaveException()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };

            //Act
            _wifeGreeterService =
                new WifeGreeterService(_context.Object, _logger.Object, _husbandServiceFactory.Object);
            var realData = await _wifeGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
    }
}

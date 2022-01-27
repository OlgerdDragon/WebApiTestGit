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
using System;
using HusbandService.Services.AdminServiceFactory;
using AdminService;

namespace HusbandService.Tests
{
    public class HusbandGreeterServiceTests
    {
        private HusbandGreeterService _husbandGreeterService;
        private Mock<IAdminServiceFactory> _adminServiceFactory = new Mock<IAdminServiceFactory>();
        private Mock<AdminGreeter.AdminGreeterClient> _adminServiceClient = new Mock<AdminGreeter.AdminGreeterClient>();

        private Mock<DbContextOptions<DbApiContext>> _optionsTown = new Mock<DbContextOptions<DbApiContext>>();
        private Mock<DbApiContext> _context = new Mock<DbApiContext>(new DbContextOptions<DbApiContext>());
        private Mock<ServerCallContext> serverCallContext = new Mock<ServerCallContext>();
        private Mock<ILogger<HusbandGreeterService>> _logger = new Mock<ILogger<HusbandGreeterService>>();

        private string userLogin = "husbandUnitTest";


        public HusbandGreeterServiceTests()
        {

        }
        private AdminService.GetProductsInShopReply GetProductsInShopReplyAdmin(List<AdminService.ProductDtoMessage> data)
        {
            var result = new AdminService.GetProductsInShopReply { Element = new AdminService.ListOfProductDto() };
            result.Element.ProductDtoMessage.AddRange(data);
            result.Successfully = true;
            return result;
        }
        private AdminService.GetProductsInShopReply GetProductsInShopReplyAdmin()
        {
            var result = new AdminService.GetProductsInShopReply
            {
                Successfully = false,
                ErrorMessage = new Mock<Exception>().ToString()
            };
            return result;
        }
        private AdminService.GetShopsForVisitReply GetShopsForVisitReplyAdmin(List<AdminService.ShopDtoMessage> data)
        {
            var result = new AdminService.GetShopsForVisitReply { Element = new AdminService.ListOfShopDto() };
            result.Element.ShopDtoMessage.AddRange(data);
            result.Successfully = true;
            return result;
        }
        private AdminService.GetShopsForVisitReply GetShopsForVisitReplyAdmin()
        {
            var result = new AdminService.GetShopsForVisitReply
            {
                Successfully = false,
                ErrorMessage = new Mock<Exception>().ToString()
            };
            return result;
        }
        private AdminService.ProductReply GetProductReplyAdmin(AdminService.ProductDtoMessage data)
        {
            var result = new AdminService.ProductReply 
            { 
                Element = data,
                Successfully = true
            };
            return result;
        }
        private AdminService.ProductReply GetProductReplyAdmin()
        {
            var result = new AdminService.ProductReply
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
        public async Task GetWantedProducts_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
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
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetWantedProducts(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Single(realData.Element.WantedProductDtoMessage);
            Assert.Equal(1, realData.Element.WantedProductDtoMessage[0].Id);
            Assert.False(realData.Element.WantedProductDtoMessage[0].BoughtStatus);
            Assert.Equal(1, realData.Element.WantedProductDtoMessage[0].ProductId);
            Assert.Equal(1, realData.Element.WantedProductDtoMessage[0].WifeId);
        }
        [Fact]
        public async Task GetWantedProducts_ShouldReturnZero_WhenZeroProduct()
        {
            //Arrange
            var data = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetWantedProducts(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.Empty(realData.Element.WantedProductDtoMessage);
        }
        [Fact]
        public async Task GetWantedProducts_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange

            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetWantedProducts(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetShopsForVisit_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
        {
            //Arrange
            var data = new List<AdminService.ShopDtoMessage>
            {
                new()
                {
                    Id = 1,
                    Name = "Metro",
                }
            };
            var resp = GetAsyncUnaryCall(GetShopsForVisitReplyAdmin(data));
            _adminServiceClient.Setup(p =>
                    p.GetShopsForVisitAsync(It.IsAny<AdminService.GetShopsForVisitRequest>(), null, null, default))
                .Returns(resp);

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
            _husbandGreeterService = 
                new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.GetShopsForVisit(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Single(realData.Element.ShopDtoMessage);
            Assert.Equal(1, realData.Element.ShopDtoMessage[0].Id);
            Assert.Equal("Metro", realData.Element.ShopDtoMessage[0].Name);
            
        }
        [Fact]
        public async Task GetShopsForVisit_ShouldReturnZero_WhenZeroWantedProduct()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());

            //Act
            _husbandGreeterService = 
                new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetShopsForVisit(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Empty(realData.Element.ShopDtoMessage);
        }
        [Fact]
        public async Task GetShopsForVisit_ShouldReturnException_WhenAdminReturnException()
        {
            //Arrange
            var resp = GetAsyncUnaryCall(GetShopsForVisitReplyAdmin());
            _adminServiceClient.Setup(p =>
                    p.GetShopsForVisitAsync(It.IsAny<AdminService.GetShopsForVisitRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _husbandGreeterService = 
                new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.GetShopsForVisit(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetShopsForVisit_ShouldReturnException_WhenHaveException()
        {
            //Arrange

            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetShopsForVisit(new UserLoginRequest { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }

        [Fact]
        public async Task GetProductsInShop_ShouldReturnOneWantedProduct_WhenOneWantedProduct()
        {
            //Arrange
            var data = new List<AdminService.ProductDtoMessage>
            {
                new()
                {
                    Id = 1,
                    Name = "Milk",
                    Price = 100,
                    ShopId = 1
                }
            };
            var itemRequest = new GetProductsInShopRequest { ShopId = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(GetProductsInShopReplyAdmin(data));
            _adminServiceClient.Setup(p =>
                    p.GetProductsInShopAsync(It.IsAny<AdminService.GetProductsInShopRequest>(), null, null, default))
                .Returns(resp);

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
            _husbandGreeterService =
                new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.GetProductsInShop(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Single(realData.Element.ProductDtoMessage);
            Assert.Equal(1, realData.Element.ProductDtoMessage[0].Id);
            Assert.Equal("Milk", realData.Element.ProductDtoMessage[0].Name);
            Assert.Equal(100, realData.Element.ProductDtoMessage[0].Price);
            Assert.Equal(1, realData.Element.ProductDtoMessage[0].ShopId);
        }
        [Fact]
        public async Task GetProductsInShop_ShouldReturnZero_WhenZeroWantedProduct()
        {
            //Arrange
            var dataWantedProduct = new List<WantedProduct>();
            var itemRequest = new GetProductsInShopRequest { ShopId = 1, UserLogin = userLogin };
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());

            //Act
            _husbandGreeterService =
                new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetProductsInShop(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Empty(realData.Element.ProductDtoMessage);
        }
        [Fact]
        public async Task GetProductsInShop_ShouldReturnException_WhenAdminReturnException()
        {
            //Arrange
            var itemRequest = new GetProductsInShopRequest { ShopId = 1, UserLogin = userLogin };
            var resp = GetAsyncUnaryCall(GetProductsInShopReplyAdmin());
            _adminServiceClient.Setup(p =>
                    p.GetProductsInShopAsync(It.IsAny<AdminService.GetProductsInShopRequest>(), null, null, default))
                .Returns(resp);

            //Act
            _husbandGreeterService = 
                new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.GetProductsInShop(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetProductsInShop_ShouldReturnException_WhenHaveException()
        {
            //Arrange
            var itemRequest = new GetProductsInShopRequest { ShopId = 1, UserLogin = userLogin };

            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetProductsInShop(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }








        [Fact]
        public async Task GetTotalAmountWantedProducts_ShouldReturn100_WhenHaveOneWantedProducts()
        {
            //Arrange
            var data = new AdminService.ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            var resp = GetAsyncUnaryCall(GetProductReplyAdmin(data));
            _adminServiceClient.Setup(p =>
                    p.GetProductAsync(It.IsAny<AdminService.ItemRequest>(), null, null, default))
                .Returns(resp);

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
            _husbandGreeterService = 
                new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(100, realData.Element);
        }
        [Fact]
        public async Task GetTotalAmountWantedProducts_ShouldReturn0_WhenHaveZeroWantedProducts()
        {
            //Arrange
            var data = new AdminService.ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            var resp = GetAsyncUnaryCall(GetProductReplyAdmin(data));
            _adminServiceClient.Setup(p =>
                    p.GetProductAsync(It.IsAny<AdminService.ItemRequest>(), null, null, default))
                .Returns(resp);
            var dataWantedProduct = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());

            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(0, realData.Element);
        }
        [Fact]
        public async Task GetTotalAmountWantedProducts_ShouldReturnException_WhenAdminReturnException()
        {
            //Arrange
            var resp = GetAsyncUnaryCall(GetProductReplyAdmin());
            _adminServiceClient.Setup(p =>
                    p.GetProductAsync(It.IsAny<AdminService.ItemRequest>(), null, null, default))

                .Returns(resp);
            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetTotalAmountWantedProducts_ShouldReturnException_WhenHaveException()
        {
            //Arrange

            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetTotalAmountWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
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
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
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
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            var wantedProductId = 1;

            //Act
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.RemoveWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
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
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnTrue_WhenZeroWantedProduct()
        {
            //Arrange
            var data = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            //Act
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.RemoveAllWantedProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetWantedProductItem_ShouldReturnTrue_WhenOneWantedProducts()
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
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(1, realData.Element.Id);
            Assert.False(realData.Element.BoughtStatus);
            Assert.Equal(1, realData.Element.ProductId);
            Assert.Equal(1, realData.Element.WifeId);
        }
        [Fact]
        public async Task GetWantedProductItem_ShouldReturnNULL_WhenZeroWantedProducts()
        {
            //Arrange
            var data = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act 
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task GetWantedProductItem_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };

            //Act 
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.GetWantedProductItem(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnTrue_WhenAddOneWantedProducts()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };

            var data = new AdminService.ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            var resp = GetAsyncUnaryCall(GetProductReplyAdmin(data));
            _adminServiceClient.Setup(p =>
                    p.GetProductAsync(It.IsAny<AdminService.ItemRequest>(), null, null, default))
                .Returns(resp);

            var dataWantedProduct = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());
            //Act 
            _husbandGreeterService = 
                new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(1, realData.Element.ProductId);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNULL_WhenZeroProducts()
        {
            //Arrange
            var itemRequest = new ItemRequest { Id = 1, UserLogin = userLogin };

            var data = new AdminService.ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            var resp = GetAsyncUnaryCall(GetProductReplyAdmin(data));
            _adminServiceClient.Setup(p =>
                    p.GetProductAsync(It.IsAny<AdminService.ItemRequest>(), null, null, default))
                .Returns(resp);

            var dataWantedProduct = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(dataWantedProduct.BuildMockDbSet());

            //Act 
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            _husbandGreeterService._adminServiceClient = _adminServiceClient.Object;
            var realData = await _husbandGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNullException_WhenNotHaveProducts()
        {
            //Arrange
            var wantedProductId = 1;

            //Act 
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            _husbandGreeterService = new HusbandGreeterService(_context.Object, _logger.Object, _adminServiceFactory.Object);
            var realData = await _husbandGreeterService.AddWantedProduct(itemRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }


    }
}

using Moq;
using Xunit;
using System;
using Grpc.Net.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Controllers;
using WifeGrpcService;
using WifeGrpcService.Data;
using WifeGrpcService.Services;
using WifeGrpcService.Models.Dto;
using WifeGrpcService.Models;
using WifeGrpcService.XUnitTests.Extensions;

namespace WifeGrpcService.XUnitTests
{
    public class WifeGreeterServiceTests
    {
        private AdminController _adminController;

        private WifeGreeterService wifeGreeterService;
        private Mock<DbContextOptions<TownContext>> _optionsTown = new Mock<DbContextOptions<TownContext>>();
        private Mock<TownContext> _context = new Mock<TownContext>(new DbContextOptions<TownContext>());
        private Mock<ILogger<WifeGreeterService>> _logger = new Mock<ILogger<WifeGreeterService>>();

        private string userLogin = "husbandUnitTest";
        public WifeGreeterServiceTests()
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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.GetWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

            //Assert
            var some = false;
            if (realData.Element.WantedProductDtoMessage.Count == 1
                    && realData.Element.WantedProductDtoMessage[0].Id == 1
                    && realData.Element.WantedProductDtoMessage[0].BoughtStatus == false
                    && realData.Element.WantedProductDtoMessage[0].ProductId == 1
                    && realData.Element.WantedProductDtoMessage[0].WifeId == 1) some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetWantedProductsAsync_ShouldReturnZeroList_WhenHaveZeroWantedProducts()
        {
            //Arrange
            var data = new List<WantedProduct>();
            _context.Setup(p => p.WantedProducts).Returns(data.BuildMockDbSet());

            //Act
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.GetWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.GetWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.GetTotalAmountWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.GetTotalAmountWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

            //Assert
            int expected = 0;
            Assert.Equal(expected, realData.Element);
        }
        [Fact]
        public async Task GetTotalAmountWantedProductsAsync_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange

            //Act
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.GetTotalAmountWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.RemoveWantedProductAsync(itemRequest);

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.RemoveWantedProductAsync(itemRequest);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveWantedProduct_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            var wantedProductId = 1;

            //Act
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.RemoveWantedProductAsync(itemRequest);

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.RemoveAllWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.RemoveAllWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveAllWantedProducts_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            //Act
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var realData = await wifeGreeterService.RemoveAllWantedProductsAsync(new UserLoginRequest() { UserLogin = userLogin });

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.GetWantedProductItemAsync(itemRequest);

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.GetWantedProductItemAsync(itemRequest);

            //Assert
            Assert.Null(realData.Element);
        }
        [Fact]
        public async Task GetWantedProductItemAsync_ShouldReturnNullException_WhenNotHaveWantedProducts()
        {
            //Arrange
            var wantedProductId = 1;

            //Act
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.GetWantedProductItemAsync(itemRequest);

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.AddProductAsync(itemRequest);

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.AddProductAsync(itemRequest);

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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var wifeGreeterService = new WifeGreeter.WifeGreeterClient(channel);
            var itemRequest = new ItemRequest { Id = wantedProductId, UserLogin = userLogin };
            var realData = await wifeGreeterService.AddProductAsync(itemRequest);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
    }
}

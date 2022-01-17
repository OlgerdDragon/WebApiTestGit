using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AdminGrpcService.Data;
using AdminGrpcService.Services;
using AdminGrpcService.Models.Dto;
using AdminGrpcService.Models;
using AdminGrpcService.XUnitTests.Extensions;
using Grpc.Core;

namespace AdminGrpcService.XUnitTests
{
    public class AdminGreeterServiceTests
    {
        private AdminGreeterService _adminService;
        private Mock<TownContext> _context = new Mock<TownContext>(new DbContextOptions<TownContext>());
        private Mock<ServerCallContext> serverCallContext = new Mock<ServerCallContext>();
        private Mock<ILogger<AdminGreeterService>> _logger = new Mock<ILogger<AdminGreeterService>>();

        private string userLogin = "adminUnitTest";
        public AdminGreeterServiceTests()
        {
            
        }
        [Fact]
        public async Task GetProductsAsync_ShouldReturnOneProduct_WhenHaveOneProduct()
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
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            var some = false;
            if (realData.Element.ProductDtoMessage.Count == 1
                    && realData.Element.ProductDtoMessage[0].Id == 1
                    && realData.Element.ProductDtoMessage[0].Name == "Salo"
                    && realData.Element.ProductDtoMessage[0].Price == 100
                    && realData.Element.ProductDtoMessage[0].ShopId == 1) some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetProductsAsync_ShouldReturnZeroList_WhenHaveZeroProducts()
        {
            //Arrange
            var data = new List<Product>();
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());

            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            var some = false;
            if (realData.Element.ProductDtoMessage.Count == 0)
                some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetProductsAsync_ShouldReturnNullException_WhenNotHaveProducts()
        {
            //Arrange
            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);
            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetShopsAsync_ShouldReturnOneShop_WhenHaveOneShops()
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
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShops(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);
            //Assert
            var some = false;
            if (realData.Element.ShopDtoMessage.Count == 1)
            {
                if (realData.Element.ShopDtoMessage[0].Id == 1 
                    && realData.Element.ShopDtoMessage[0].Name == "Metro") some = true;
            }
            Assert.True(some);
        }
        [Fact]
        public async Task GetShopsAsync_ShouldReturnZeroList_WhenHaveZeroShops()
        {
            //Arrange
            var data = new List<Shop>();
            _context.Setup(p => p.Shops).Returns(data.BuildMockDbSet());
            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShops(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);
            //Assert
            var some = false;
            if (realData.Element.ShopDtoMessage.Count == 0)
                some = true;
            Assert.True(some);
        }
        [Fact]
        public async Task GetShopsAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShops(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);
            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task UpdateShopAsync_ShouldReturnOneShop_WhenHaveOneShops()
        {
            //Arrange
            var data = new List<Shop>
            {
                new()
                {
                    Id = 1,
                Name = ""
                }

            };
            var shopDto = new ShopDtoMessage
            {
                Id = 1,
                Name = "Metro"
            };
            _context.Setup(p => p.Shops).Returns(data.BuildMockDbSet());
            //Act
            var shopRequest = new ShopRequest { ShopDtoMessage = shopDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateShop(shopRequest, serverCallContext.Object);
            //Assert
            var some = false;
            if (realData.Element.Id == 1
                && realData.Element.Name == "Metro") some = true;
            Assert.True(some);
        }
        [Fact]
        public async Task UpdateShopAsync_ShouldReturnNullShopDto_WhenNullShopDto()
        {
            //Arrange
            var dataShop = new List<Shop>
            {
                new()
                {
                    Id = 1,
                    Name = "Metro"
                }
            };
            var shopDto = new ShopDtoMessage();
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            var shopRequest = new ShopRequest { ShopDtoMessage = shopDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateShop(shopRequest, serverCallContext.Object);
            //Assert
            var expected = 0;
            Assert.Equal(expected, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateShopAsync_ShouldReturnNullShopDto_WhenNotHaveShopDto()
        {
            //Arrange
            var dataShop = new List<Shop>
            {
                new()
                {
                    Id = 1,
                    Name = "Metro"
                }
            };
            var shopDto = new ShopDtoMessage
            {
                Id = 2,
                Name = "Metro"
            };
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            var shopRequest = new ShopRequest { ShopDtoMessage = shopDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateShop(shopRequest, serverCallContext.Object);
            //Assert 
            var expected = 0;
            Assert.Equal(expected, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateShopAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopDto = new ShopDtoMessage
            {
                Id = 1,
                Name = "Metro"
            };

            //Act
            var shopRequest = new ShopRequest { ShopDtoMessage = shopDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateShop(shopRequest, serverCallContext.Object);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task UpdateProductAsync_ShouldReturnOneShop_WhenHaveOneShops()
        {
            //Arrange
            var data = new List<Product>
            {
                new()
                {
                     Id = 1,
                    Name = "",
                    Price = 0,
                    ShopId =0
                }
            };
            var producDto = new ProductDtoMessage
            {
                Id = 1,
                Name = "Metro",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateProduct(productRequest, serverCallContext.Object);
            //Assert
            var some = false;
            if (realData.Element.Id == 1
                && realData.Element.Name == "Metro") some = true;
            Assert.True(some);
        }
        [Fact]
        public async Task UpdateProductAsync_ShouldReturnNullProductDto_WhenNullProductDto()
        {
            //Arrange
            var data = new List<Product>
            {
                new()
                {
                     Id = 1,
                    Name = "",
                    Price = 0,
                    ShopId =0
                }
            };
            var producDto = new ProductDtoMessage();
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateProduct(productRequest, serverCallContext.Object);
            //Assert
            var expected = 0;
            Assert.Equal(expected, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateProductAsync_ShouldReturnNullProductDto_WhenNotHaveProductDto()
        {
            //Arrange
            var data = new List<Product>
            {
                new()
                {
                     Id = 1,
                    Name = "",
                    Price = 0,
                    ShopId =0
                }
            };
            var producDto = new ProductDtoMessage
            {
                Id = 2,
                Name = "Bread",
                Price = 10,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateProduct(productRequest, serverCallContext.Object);
            //Assert
            var expected = 0;
            Assert.Equal(expected, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateProductAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var producDto = new ProductDtoMessage
            {
                Id = 1,
                Name = "Metro",
                Price = 100,
                ShopId = 1
            };
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateProduct(productRequest, serverCallContext.Object);
            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnTrue_WhenOneProductDto()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var shop = new Shop
            {
                Id = 1,
                Name = "Name"
            };
            var producDto = new ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };

            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            _context.Setup(p => p.Shops.FindAsync(producDto.Id)).Returns(new ValueTask<Shop>(shop));
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(productRequest, serverCallContext.Object);

            //Assert

            Assert.True(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnFalse_WhenNotHaveNeededShop()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var dataShop = new List<Shop>();
            var producDto = new ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(productRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnFalse_WhenNotHaveShops()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var producDto = new ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(productRequest, serverCallContext.Object);
            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnFalse_WhenNULLProductDto()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var dataShop = new List<Shop>();
            var producDto = new ProductDtoMessage();

            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(productRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNullException_WhenNotHaveProducts()
        {
            var dataShop = new List<Shop>();
            //Arrange
            var producDto = new ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(productRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var producDto = new ProductDtoMessage
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            //Act
            var productRequest = new ProductRequest { ProductDtoMessage = producDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(productRequest, serverCallContext.Object);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task AddShop_ShouldReturnTrue_WhenAddOneWantedProduct()
        {
            //Arrange
            var dataShop = new List<Shop>();
            var shopDto = new ShopDtoMessage
            {
                Id = 1,
                Name = "ATB",
            };

            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());

            //Act
            var shopRequest = new ShopRequest { ShopDtoMessage = shopDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddShop(shopRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task AddShop_ShouldReturnFalse_WhenNULLShopDto()
        {
            //Arrange
            var dataShop = new List<Shop>();
            var shopDto = new ShopDtoMessage();

            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            var shopRequest = new ShopRequest { ShopDtoMessage = shopDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddShop(shopRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddShop_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopDto = new ShopDtoMessage
            {
                Id = 1,
                Name = "ATB",
            };
            //Act
            var shopRequest = new ShopRequest { ShopDtoMessage = shopDto, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.AddShop(shopRequest, serverCallContext.Object);
            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task RemoveProduct_ShouldReturnTrue_When1()
        {
            //Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            var productId = 1;
            _context.Setup(p => p.Products.FindAsync(productId)).Returns(new ValueTask<Product>(product));
            //Act
            var itemRquest = new ItemRequest { Id = productId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveProduct(itemRquest, serverCallContext.Object);
            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveProduct_ShouldReturnFalse_WhenNotHaveNeededProduct()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var productId = 1;
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            //Act
            var itemRquest = new ItemRequest { Id = productId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveProduct(itemRquest, serverCallContext.Object);
            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveProduct_ShouldReturnNullException_WhenNotHaveProducts()
        {
            //Arrange
            var producDto = new ProductDto
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            var productId = 1;
            //Act
            var itemRquest = new ItemRequest { Id = productId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveProduct(itemRquest, serverCallContext.Object);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task RemoveShop_ShouldReturnTrue_When1()
        {
            //Arrange
            var shop = new Shop
            {
                Id = 1,
                Name = "ATB"
            };
            var shopId = 1;
            _context.Setup(p => p.Shops.FindAsync(shopId)).Returns(new ValueTask<Shop>(shop));

            //Act
            var itemRquest = new ItemRequest { Id = shopId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveShop(itemRquest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task RemoveShop_ShouldReturnFalse_WhenNotHaveNeededShop()
        {
            //Arrange
            var dataShop = new List<Shop>();
            var shopId = 1;
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            var itemRquest = new ItemRequest { Id = shopId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveShop(itemRquest, serverCallContext.Object);
            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveShop_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopId = 1;
            //Act
            var itemRquest = new ItemRequest { Id = shopId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveShop(itemRquest, serverCallContext.Object);
            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetShopAsync_ShouldReturnOneShopDto_When1()
        {
            //Arrange
            var shop = new Shop
            {
                Id = 1,
                Name = "ATB"
            };
            var shopId = 1;
            _context.Setup(p => p.Shops.FindAsync(shopId)).Returns(new ValueTask<Shop>(shop));

            //Act
            var itemRquest = new ItemRequest { Id = shopId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShop(itemRquest, serverCallContext.Object);

            //Assert
            var some = false;
            if (realData.Element.Id == 1
                && realData.Element.Name == "ATB") some = true;
            Assert.True(some);
        }
        [Fact]
        public async Task GetShopAsync_ShouldReturnNULL_WhenNotHaveNeededShop()
        {
            //Arrange
            var dataShop = new List<Shop>();
            var shopId = 1;
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            var itemRquest = new ItemRequest { Id = shopId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShop(itemRquest, serverCallContext.Object);
            //Assert
            Assert.Null(realData);
        }
        [Fact]
        public async Task GetShopAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopId = 1;
            //Act
            var itemRquest = new ItemRequest { Id = shopId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShop(itemRquest, serverCallContext.Object);

            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetProductAsync_ShouldReturnOneProduct_When1()
        {
            //Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            var productId = 1;
            _context.Setup(p => p.Products.FindAsync(productId)).Returns(new ValueTask<Product>(product));
            //Act
            var itemRquest = new ItemRequest { Id = productId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProduct(itemRquest, serverCallContext.Object);
            //Assert
            var some = false;
            if (realData.Element.Id == 1
                && realData.Element.Name == "Milk"
                && realData.Element.Price == 100
                && realData.Element.ShopId == 1) some = true;
            Assert.True(some);
        }
        [Fact]
        public async Task GetProductAsync_ShouldReturnNULL_WhenNotHaveNeededProduct()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var productId = 1;
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            //Act
            var itemRquest = new ItemRequest { Id = productId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProduct(itemRquest, serverCallContext.Object);
            //Assert
            Assert.Null(realData);
        }
        [Fact]
        public async Task GetProductAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var productId = 1;
            //Act
            var itemRquest = new ItemRequest { Id = productId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProduct(itemRquest, serverCallContext.Object);
            //Assert
            Assert.NotNull(realData.ErrorMessage);
        }
    }
}

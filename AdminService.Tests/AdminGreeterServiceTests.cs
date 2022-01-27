using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AdminService.Services;
using AdminService.Models.Dto;
using AdminService.Tests.Extensions;
using Grpc.Core;
using DbApiContextForService;
using DbApiContextForService.Models;

namespace AdminService.Tests
{
    public class AdminGreeterServiceTests
    {
        private AdminGreeterService _adminService;
        private Mock<DbApiContext> _context = new Mock<DbApiContext>(new DbContextOptions<DbApiContext>());
        private Mock<ServerCallContext> serverCallContext = new Mock<ServerCallContext>();
        private Mock<ILogger<AdminGreeterService>> _logger = new Mock<ILogger<AdminGreeterService>>();

        private string userLogin = "adminUnitTest";
        public AdminGreeterServiceTests()
        {
            
        }
        [Fact]
        public async Task GetProducts_ShouldReturnOneProduct_WhenHaveOneProduct()
        {
            //Arrange
            var data = new List<Product>
            {
                new()
                {
                    Id=1,
                    Name = "Milk",
                    Price = 100,
                    ShopId = 1
                }
            };
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());

            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Single(realData.Element.ProductDtoMessage);
            Assert.Equal(1, realData.Element.ProductDtoMessage[0].Id);
            Assert.Equal("Milk", realData.Element.ProductDtoMessage[0].Name);
            Assert.Equal(100, realData.Element.ProductDtoMessage[0].Price);
            Assert.Equal(1, realData.Element.ProductDtoMessage[0].ShopId);
        }
        [Fact]
        public async Task GetProducts_ShouldReturnZeroList_WhenHaveZeroProducts()
        {
            //Arrange
            var data = new List<Product>();
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());

            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Empty(realData.Element.ProductDtoMessage);
        }
        [Fact]
        public async Task GetProducts_ShouldReturnNullException_WhenNotHaveProducts()
        {
            //Arrange
            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProducts(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);
            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetShops_ShouldReturnOneShop_WhenHaveOneShops()
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
            Assert.True(realData.Successfully);
            Assert.Single(realData.Element.ShopDtoMessage);
            Assert.Equal(1, realData.Element.ShopDtoMessage[0].Id);
            Assert.Equal("Metro", realData.Element.ShopDtoMessage[0].Name);
        }
        [Fact]
        public async Task GetShops_ShouldReturnZeroList_WhenHaveZeroShops()
        {
            //Arrange
            var data = new List<Shop>();
            _context.Setup(p => p.Shops).Returns(data.BuildMockDbSet());
            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShops(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);
            //Assert
            Assert.True(realData.Successfully);
            Assert.Empty(realData.Element.ShopDtoMessage);
        }
        [Fact]
        public async Task GetShops_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShops(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);
            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task UpdateShop_ShouldReturnOneShop_WhenHaveOneShops()
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
            
        }
        [Fact]
        public async Task UpdateShop_ShouldReturnNullShopDto_WhenNullShopDto()
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
            Assert.False(realData.Successfully);
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateShop_ShouldReturnNullShopDto_WhenNotHaveShopDto()
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
            Assert.False(realData.Successfully);
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateShop_ShouldReturnNullException_WhenNotHaveShops()
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
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task UpdateProduct_ShouldReturnOneShop_WhenHaveOneShops()
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
            Assert.True(realData.Successfully);
            Assert.Equal(1, realData.Element.Id);
            Assert.Equal("Metro", realData.Element.Name);
        }
        [Fact]
        public async Task UpdateProduct_ShouldReturnNullProductDto_WhenNullProductDto()
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
            Assert.False(realData.Successfully);
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateProduct_ShouldReturnNullProductDto_WhenNotHaveProductDto()
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
            Assert.False(realData.Successfully);
            Assert.Equal(0, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateProduct_ShouldReturnNullException_WhenNotHaveShops()
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
            Assert.False(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.False(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.False(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.False(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.False(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.True(realData.Successfully);
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
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetShop_ShouldReturnOneShopDto_When1()
        {
            //Arrange
            var shop = new Shop
            {
                Id = 1,
                Name = "Metro"
            };
            var shopId = 1;
            _context.Setup(p => p.Shops.FindAsync(shopId)).Returns(new ValueTask<Shop>(shop));

            //Act
            var itemRquest = new ItemRequest { Id = shopId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShop(itemRquest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Equal(1, realData.Element.Id);
            Assert.Equal("Metro", realData.Element.Name);
        }
        [Fact]
        public async Task GetShop_ShouldReturnNULL_WhenNotHaveNeededShop()
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
            Assert.True(realData.Successfully);
            Assert.Null(realData.Element);
        }
        [Fact]
        public async Task GetShop_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopId = 1;
            //Act
            var itemRquest = new ItemRequest { Id = shopId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShop(itemRquest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetProduct_ShouldReturnOneProduct_When1()
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
            Assert.True(realData.Successfully);
            Assert.Equal(1, realData.Element.Id);
            Assert.Equal("Milk", realData.Element.Name);
            Assert.Equal(100, realData.Element.Price);
            Assert.Equal(1, realData.Element.ShopId);
        }
        [Fact]
        public async Task GetProduct_ShouldReturnNULL_WhenNotHaveNeededProduct()
        {
            //Arrange
            var dataProduct = new List<Product>();
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            //Act
            var itemRquest = new ItemRequest { Id = 1, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProduct(itemRquest, serverCallContext.Object);
            //Assert
            Assert.True(realData.Successfully);
            Assert.Null(realData.Element);
        }
        [Fact]
        public async Task GetProduct_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var productId = 1;
            //Act
            var itemRquest = new ItemRequest { Id = productId, UserLogin = userLogin };
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProduct(itemRquest, serverCallContext.Object);
            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }








        [Fact]
        public async Task GetShopsForVisit_ShouldReturnOneProduct_When1()
        {
            //Arrange
            var shop = new Shop
            {
                Id = 1,
                Name = "ATB"
            };
            _context.Setup(p => p.Shops.FindAsync(shop.Id)).Returns(new ValueTask<Shop>(shop));
            var product = new Product
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products.FindAsync(product.Id)).Returns(new ValueTask<Product>(product));
            
            //Act
            var getShopsForVisitRequest = new GetShopsForVisitRequest 
            { 
                WantedProductList = new ListOfWantedProductDto(),
                UserLogin = userLogin 
            };
            var wantedProductDtoList = new List<WantedProductDtoMessage>
            { 
                new WantedProductDtoMessage
                {
                    Id =1,                    
                    BoughtStatus =false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            getShopsForVisitRequest.WantedProductList.WantedProductDtoMessage.AddRange(wantedProductDtoList);
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsForVisit(getShopsForVisitRequest, serverCallContext.Object);
            
            //Assert
            Assert.True(realData.Successfully);
            Assert.Single(realData.Element.ShopDtoMessage);
            Assert.Equal(1, realData.Element.ShopDtoMessage[0].Id);
            Assert.Equal("ATB", realData.Element.ShopDtoMessage[0].Name);
        }
        [Fact]
        public async Task GetShopsForVisit_ShouldReturnException_WhenNotHaveProduct()
        {
            //Arrange
            var shop = new Shop
            {
                Id = 1,
                Name = "ATB"
            };
            _context.Setup(p => p.Shops.FindAsync(shop.Id)).Returns(new ValueTask<Shop>(shop));
            var dataProduct = new List<Product>();
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            
            //Act
            var getShopsForVisitRequest = new GetShopsForVisitRequest
            {
                WantedProductList = new ListOfWantedProductDto(),
                UserLogin = userLogin
            };
            var wantedProductDtoList = new List<WantedProductDtoMessage>
            {
                new WantedProductDtoMessage
                {
                    Id =1,
                    BoughtStatus =false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            getShopsForVisitRequest.WantedProductList.WantedProductDtoMessage.AddRange(wantedProductDtoList);
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsForVisit(getShopsForVisitRequest, serverCallContext.Object);
            
            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetShopsForVisit_ShouldReturnException_WhenNotHaveShop()
        {
            //Arrange
            var shop = new List<Shop>();
            _context.Setup(p => p.Shops).Returns(shop.BuildMockDbSet());
            var product = new Product
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products.FindAsync(product.Id)).Returns(new ValueTask<Product>(product));
            var dataProduct = new List<Product>();
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            
            //Act
            var getShopsForVisitRequest = new GetShopsForVisitRequest
            {
                WantedProductList = new ListOfWantedProductDto(),
                UserLogin = userLogin
            };
            var wantedProductDtoList = new List<WantedProductDtoMessage>
            {
                new WantedProductDtoMessage
                {
                    Id =1,
                    BoughtStatus =false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            getShopsForVisitRequest.WantedProductList.WantedProductDtoMessage.AddRange(wantedProductDtoList);
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsForVisit(getShopsForVisitRequest, serverCallContext.Object);
            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetShopsForVisit_ShouldReturnException_WhenHaveException()
        {
            //Arrange
            var getShopsForVisitRequest = new GetShopsForVisitRequest
            {
                WantedProductList = new ListOfWantedProductDto(),
                UserLogin = userLogin
            };
            var wantedProductDtoList = new List<WantedProductDtoMessage>
            {
                new WantedProductDtoMessage
                {
                    Id =1,
                    BoughtStatus =false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            getShopsForVisitRequest.WantedProductList.WantedProductDtoMessage.AddRange(wantedProductDtoList);

            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsForVisit(getShopsForVisitRequest, serverCallContext.Object);
            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }












        [Fact]
        public async Task GetProductsInShop_ShouldReturnOneProduct_When1()
        {
            //Arrange
            var shop = new Shop
            {
                Id = 1,
                Name = "ATB"
            };
            _context.Setup(p => p.Shops.FindAsync(shop.Id)).Returns(new ValueTask<Shop>(shop));
            var product = new Product
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products.FindAsync(product.Id)).Returns(new ValueTask<Product>(product));

            //Act
            var getShopsForVisitRequest = GetProductsInShopRequest();
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsInShop(getShopsForVisitRequest, serverCallContext.Object);

            //Assert
            Assert.True(realData.Successfully);
            Assert.Single(realData.Element.ProductDtoMessage);
            Assert.Equal(1, realData.Element.ProductDtoMessage[0].Id);
            Assert.Equal("Milk", realData.Element.ProductDtoMessage[0].Name);
            Assert.Equal(100, realData.Element.ProductDtoMessage[0].Price);
            Assert.Equal(1, realData.Element.ProductDtoMessage[0].ShopId);
        }
        [Fact]
        public async Task GetProductsInShop_ShouldReturnException_WhenNotHaveProduct()
        {
            //Arrange
            var shop = new Shop
            {
                Id = 1,
                Name = "ATB"
            };
            _context.Setup(p => p.Shops.FindAsync(shop.Id)).Returns(new ValueTask<Shop>(shop));
            var dataProduct = new List<Product>();
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());

            //Act
            var getShopsForVisitRequest = GetProductsInShopRequest();
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsInShop(getShopsForVisitRequest, serverCallContext.Object);

            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetProductsInShop_ShouldReturnException_WhenNotHaveShop()
        {
            //Arrange
            var shop = new List<Shop>();
            _context.Setup(p => p.Shops).Returns(shop.BuildMockDbSet());
            var product = new Product
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products.FindAsync(product.Id)).Returns(new ValueTask<Product>(product));
            var dataProduct = new List<Product>();
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());

            //Act
            var getShopsForVisitRequest = GetProductsInShopRequest();
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsInShop(getShopsForVisitRequest, serverCallContext.Object);
            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        [Fact]
        public async Task GetProductsInShop_ShouldReturnException_WhenHaveException()
        {
            //Arrange
            var getShopsForVisitRequest = GetProductsInShopRequest();

            //Act
            _adminService = new AdminGreeterService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsInShop(getShopsForVisitRequest, serverCallContext.Object);
            //Assert
            Assert.False(realData.Successfully);
            Assert.NotNull(realData.ErrorMessage);
        }
        private GetProductsInShopRequest GetProductsInShopRequest()
        {
            var getProductsInShopRequest = new GetProductsInShopRequest
            {
                ShopId = 1,
                WantedProductList = new ListOfWantedProductDto(),
                UserLogin = userLogin
            };
            var wantedProductDtoList = new List<WantedProductDtoMessage>
            {
                new WantedProductDtoMessage
                {
                    Id =1,
                    BoughtStatus =false,
                    ProductId = 1,
                    WifeId = 1
                }
            };
            getProductsInShopRequest.WantedProductList.WantedProductDtoMessage.AddRange(wantedProductDtoList);
            return getProductsInShopRequest;
        }
    }
}

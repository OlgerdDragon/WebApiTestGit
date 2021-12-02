﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WebApiMonolit.Data;
using WebApiMonolit.Services.AdminService;
using Microsoft.EntityFrameworkCore;
using WebApiMonolit.Models;
using WebApiMonolit.Models.Dto;
using WebApiMonolit.XUnitTests.Extensions;


namespace WebApiMonolit.XUnitTests.Common
{
    public class AdminServiceTests
    {
        private IAdminService _adminService;
        private Mock<TownContext> _context = new Mock<TownContext>(new DbContextOptions<TownContext>());
        private Mock<ILogger<AdminService>> _logger = new Mock<ILogger<AdminService>>();

        private string userLogin = "adminUnitTest";
        public AdminServiceTests()
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsAsync();

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
        public async Task GetProductsAsync_ShouldReturnZeroList_WhenHaveZeroProducts()
        {
            //Arrange
            var data = new List<Product>();

            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());

            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsAsync();

            //Assert
            var some = false;
            if (realData.Element.Count == 0)
                some = true;

            Assert.True(some);
        }
        [Fact]
        public async Task GetProductsAsync_ShouldReturnNullException_WhenNotHaveProducts()
        {
            //Arrange
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductsAsync();
            //Assert
            Assert.NotNull(realData.ExceptionMessage);
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
        [Fact]
        public async Task GetShopsAsync_ShouldReturnZeroList_WhenHaveZeroShops()
        {
            //Arrange
            var data = new List<Shop>();
            _context.Setup(p => p.Shops).Returns(data.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsAsync();
            //Assert
            var some = false;
            if (realData.Element.Count == 0)
                some = true;
            Assert.True(some);
        }
        [Fact]
        public async Task GetShopsAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopsAsync();
            //Assert
            Assert.NotNull(realData.ExceptionMessage);
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
            var dataDto = new ShopDto
            {
                Id = 1,
                Name = "Metro"
            };
            _context.Setup(p => p.Shops).Returns(data.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateShopAsync(dataDto, userLogin);
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
            var dataDto = new ShopDto();
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateShopAsync(dataDto, userLogin);
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
            var dataDto = new ShopDto
            {
                Id = 2,
                Name = "Metro"
            };
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateShopAsync(dataDto, userLogin);
            //Assert
            var expected = 0;
            Assert.Equal(expected, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateShopAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var dataDto = new ShopDto
            {
                Id = 1,
                Name = "Metro"
            };

            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateShopAsync(dataDto, userLogin);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
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
            var dataDto = new ProductDto
            {
                Id = 1,
                Name = "Metro",
                Price = 100,
                ShopId =1
            };
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateProductAsync(dataDto, userLogin);
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
            var dataDto = new ProductDto();
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateProductAsync(dataDto, userLogin);
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
            var dataDto = new ProductDto
            {
                Id = 2,
                Name = "Bread",
                Price = 10,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(data.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateProductAsync(dataDto, userLogin);
            //Assert
            var expected = 0;
            Assert.Equal(expected, realData.Element.Id);
        }
        [Fact]
        public async Task UpdateProductAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var dataDto = new ProductDto
            {
                Id = 1,
                Name = "Metro",
                Price = 100,
                ShopId = 1
            };
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.UpdateProductAsync(dataDto, userLogin);
            //Assert
            Assert.NotNull(realData.ExceptionMessage);
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
            var producDto = new ProductDto
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };

            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            _context.Setup(p => p.Shops.FindAsync(producDto.Id)).Returns(new ValueTask<Shop>(shop));
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(producDto, userLogin);

            //Assert

            Assert.True(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnFalse_WhenNotHaveNeededShop()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var dataShop = new List<Shop>();
            var producDto = new ProductDto
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(producDto, userLogin);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnFalse_WhenNotHaveShops()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var producDto = new ProductDto
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(producDto, userLogin);
            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnFalse_WhenNULLProductDto()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var dataShop = new List<Shop>();
            var producDto = new ProductDto();

            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(producDto, userLogin);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNullException_WhenNotHaveProducts()
        {
            var dataShop = new List<Shop>();
            //Arrange
            var producDto = new ProductDto
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(producDto, userLogin);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var dataProduct = new List<Product>();
            var producDto = new ProductDto
            {
                Id = 1,
                Name = "Milk",
                Price = 100,
                ShopId = 1
            };
            _context.Setup(p => p.Products).Returns(dataProduct.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddProduct(producDto, userLogin);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
        }
        [Fact]
        public async Task AddShop_ShouldReturnTrue_WhenAddOneWantedProduct()
        {
            //Arrange
            var dataShop = new List<Shop>();
            var shopDto = new ShopDto
            {
                Id = 1,
                Name = "ATB",
            };

            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());

            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddShop(shopDto, userLogin);

            //Assert
            Assert.True(realData.Element);
        }
        [Fact]
        public async Task AddShop_ShouldReturnFalse_WhenNULLShopDto()
        {
            //Arrange
            var dataShop = new List<Shop>();
            var shopDto = new ShopDto();

            _context.Setup(p => p.Shops).Returns(dataShop.BuildMockDbSet());
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddShop(shopDto, userLogin);

            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task AddShop_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopDto = new ShopDto
            {
                Id = 1,
                Name = "ATB",
            };
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.AddShop(shopDto, userLogin);
            //Assert
            Assert.NotNull(realData.ExceptionMessage);
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveProduct(productId, userLogin);
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveProduct(productId, userLogin);
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveProduct(productId, userLogin);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveShop(shopId, userLogin);

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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveShop(shopId, userLogin);
            //Assert
            Assert.False(realData.Element);
        }
        [Fact]
        public async Task RemoveShop_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopId = 1;
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.RemoveShop(shopId, userLogin);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopAsync(shopId);

            //Assert
            var some = false;
            if (realData.Element.Value.Id == 1
                && realData.Element.Value.Name == "ATB") some = true;
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopAsync(shopId);
            //Assert
            Assert.Null(realData);
        }
        [Fact]
        public async Task GetShopAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var shopId = 1;
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetShopAsync(shopId);

            //Assert
            Assert.NotNull(realData.ExceptionMessage);
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductAsync(productId);
            //Assert
            var some = false;
            if (realData.Element.Value.Id == 1
                && realData.Element.Value.Name == "Milk"
                && realData.Element.Value.Price == 100
                && realData.Element.Value.ShopId == 1) some = true;
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
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductAsync(productId);
            //Assert
            Assert.Null(realData);
        }
        [Fact]
        public async Task GetProductAsync_ShouldReturnNullException_WhenNotHaveShops()
        {
            //Arrange
            var productId = 1;
            //Act
            _adminService = new AdminService(_context.Object, _logger.Object);
            var realData = await _adminService.GetProductAsync(productId);
            //Assert
            Assert.NotNull(realData.ExceptionMessage);
        }
    }
}

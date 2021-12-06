using System;
using System.Net;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AdminGrpcService.Services;
using System.Collections.Generic;
using AdminGrpcService.Models;
using WebApiGeneralGrpcTests.IntegrationXUnitTest.Extensions;
using WebApiGeneralGrpc.Controllers;
using AdminGrpcService;
using Grpc.Core;
using Microsoft.CodeAnalysis;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class AdminControllerIntegrationTests : BaseIntegrationTests
    {
        private AdminGreeterService _adminService;
        private AdminController _adminController;
        //private IAdminService _adminService;

        private Mock<WebApiGeneralGrpc.Data.TownContext> _context = new Mock<WebApiGeneralGrpc.Data.TownContext>(new DbContextOptions<WebApiGeneralGrpc.Data.TownContext>());
        private Mock<ILogger<WebApiGeneralGrpc.Services.AdminService.AdminService>> _logger = new Mock<ILogger<WebApiGeneralGrpc.Services.AdminService.AdminService>>();
        private Mock<AdminGreeter.AdminGreeterClient> _greeterClient = new Mock<AdminGreeter.AdminGreeterClient>();
        private Mock<WebApplicationFactory<WebApiGeneralGrpc.Startup>> _generalHost = new Mock<WebApplicationFactory<WebApiGeneralGrpc.Startup>>();
        private Mock<AdminController> _adminControllerMock = new Mock<AdminController>();
        private Mock<ServerCallContext> serverCallContext = new Mock<ServerCallContext>();

        private Mock<AdminGrpcService.Data.TownContext> _adminContext = new Mock<AdminGrpcService.Data.TownContext>(new DbContextOptions<AdminGrpcService.Data.TownContext>());
        private Mock<ILogger<AdminGrpcService.Services.AdminGreeterService>> _adminLogger = new Mock<ILogger<AdminGrpcService.Services.AdminGreeterService>>();
        private string userLogin = "adminUnitTest";


        //public virtual async AsyncUnaryCall<GetShopsReply> asyncUnaryCall()
        //{
        //    var a = await _adminService.GetShops(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);
        //    return new Task<GetShopsReply>(a);
        //}
                
        [Fact]
        public async void Test1()
        {
            
            var adminHost = new WebApplicationFactory<WebApiGeneralGrpc.Startup>().WithWebHostBuilder(_ => { });
            var httpClient = webHost.CreateClient();
            
            var response = await httpClient.GetAsync("Api/Admin/ShopsM");  
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //private readonly TestServer _server;
        //private readonly HttpClient _client;

        //public AccountControllerIntegrationTests()
        //{
        //    // Arrange
        //    //_server = new TestServer(new WebHostBuilder()
        //    //   .UseStartup<Startup>());
        //    //_client = _server.CreateClient();
        //}

        ////[Fact]
        //public async Task ReturnHelloWorld()
        //{
        //    // Act
        //    //var response = await _client.GetAsync("Api/Admin/ShopsM");
        //    //response.EnsureSuccessStatusCode();
        //    //var responseString = await response.Content.ReadAsStringAsync(); 
        //    //// Assert
        //    //Assert.Equal("Hello World!", responseString);
        //}
        [Fact]
        public async void TestWithMock()
        {
            
            var adminHost = new WebApplicationFactory<AdminGrpcService.Startup>().WithWebHostBuilder(_ => { });
            var adminHttpClient = adminHost.CreateClient();

            var data = new List<Product>();

            _adminContext.Setup(p => p.Products).Returns(data.BuildMockDbSet());
            _adminService = new AdminGreeterService(_adminContext.Object, _adminLogger.Object);

            //_adminControllerMock.Setup(p => p.adminServiceClient).Returns(_greeterClient.Object);
            var userLoginReques = new UserLoginRequest() { UserLogin = userLogin };
            var getShopReply = await _adminService.GetShops(new UserLoginRequest() { UserLogin = userLogin }, serverCallContext.Object);

            _greeterClient.Setup(p => p.GetShopsAsync(userLoginReques, null, null, default(System.Threading.CancellationToken)))
                .Returns(getShopReply);

            _adminControllerMock.Setup(p => p.adminServiceClient).Returns(_greeterClient.Object);
            var element = _adminControllerMock.Object.GetShopItemsM();
            var generalHttpClient = _generalHost.Object.CreateClient();

            var response = await generalHttpClient.GetAsync("Api/Admin/ShopsM");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Shops_ClientAndClient()
        { 
        }
        
    }
}

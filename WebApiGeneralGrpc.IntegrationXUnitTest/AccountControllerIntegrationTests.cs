using System;
using System.Net;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using AdminGrpcService;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class AccountControllerIntegrationTests : BaseIntegrationTests
    {
        [Fact]
        public async void Test1()
        {
            
            var adminHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            var httpClient = webHost.CreateClient();
            
            var response = await httpClient.GetAsync("Api/Admin/ShopsM");  
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AccountControllerIntegrationTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ReturnHelloWorld()
        {
            // Act
            var response = await _client.GetAsync("Api/Admin/ShopsM");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal("Hello World!", responseString);
        }
    }
}

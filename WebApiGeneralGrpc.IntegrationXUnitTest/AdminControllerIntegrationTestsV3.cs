using System;
using System.Net;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using AdminGrpcService;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using FluentAssertions;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class AdminControllerIntegrationTestsV3 : BaseIntegrationTestsV3
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            // Arrange
            //await AuthenticateAsync();

            // Act
            
            var response = await globalTestClient.GetAsync("Api/Admin/Shops");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            //(await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
        }
    }
}

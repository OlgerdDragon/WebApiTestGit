using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class ServiceConnectionTests : BaseIntegrationTests
    {
        [Fact]
        public async Task AdminHealthTest()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("/health/admin");
            Assert.True(res.IsSuccessStatusCode);
        }
    }
}
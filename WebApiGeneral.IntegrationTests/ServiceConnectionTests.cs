using System.Threading.Tasks;
using Xunit;

namespace WebApiGeneral.IntegrationTests
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
        [Fact]
        public async Task HusbandHealthTest()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("/health/husband");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task WifeHealthTest()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("/health/wife");
            Assert.True(res.IsSuccessStatusCode);
        }
        
    }
}
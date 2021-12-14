using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class HusbandConnectionTests : BaseIntegrationTests
    {
        [Fact]
        public async Task GetNeededProductList_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Husband/Products");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GetNeededShopList_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Husband/Shops");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GetNededProductListInShop_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Husband/ProductsInShop/1");
            Assert.True(res.IsSuccessStatusCode);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class WifeConnectionTests : BaseIntegrationTests
    {
        [Fact]
        public async Task GetWantedProducts_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Wife/Products");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GetTotalAmountWantedProducts_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Wife/Products/TotalAmount");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GetWantedProductItem_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Wife/Product/1");
            Assert.True(res.IsSuccessStatusCode);
        }
        //[Fact]
        //public async Task CreateWantedProductItem_ConnectionTests()
        //{
        //    var client = factory.CreateDefaultClient();
        //    var res = await client.PostAsync("Api/Wife/Products");
        //    Assert.True(res.IsSuccessStatusCode);
        //}
        [Fact]
        public async Task DeleteWantedProductItem_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.DeleteAsync("Api/Wife/Product/1");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task DeleteAllProductItem_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.DeleteAsync("Api/Wife/Products");
            Assert.True(res.IsSuccessStatusCode);
        }
    }
}

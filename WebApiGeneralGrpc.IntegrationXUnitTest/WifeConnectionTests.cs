using AdminGrpcService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var message = new ProductDtoMessage { Id = 1, Name = "Milk", Price = 100, ShopId = 1 };
            var myContent = JsonConvert.SerializeObject(message);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");





            
            var client = factory.CreateDefaultClient();
            var resAdd = await client.PostAsync("Api/Admin/Product", byteContent);
            var z = resAdd.Content;
           
            var res = await client.GetAsync("Api/Admin/Products");
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
        public async Task DeleteAllProductItem_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.DeleteAsync("Api/Wife/Products");
            Assert.True(res.IsSuccessStatusCode);
        }
        
    }
}

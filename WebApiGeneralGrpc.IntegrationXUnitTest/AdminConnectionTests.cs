﻿using AdminGrpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class AdminConnectionTests : BaseIntegrationTests
    {
        [Fact]
        public async Task GetShopItems_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Admin/Shops");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GetProductItems_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Admin/Products");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GetShopItem_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Admin/Shop/1");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GetProductItem_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.GetAsync("Api/Admin/Product/1");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task DeleteShopItem_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.DeleteAsync("Api/Admin/Shop/1");
            Assert.True(res.IsSuccessStatusCode);
        }
        [Fact]
        public async Task DeleteProductItem_ConnectionTests()
        {
            var client = factory.CreateDefaultClient();
            var res = await client.DeleteAsync("Api/Admin/Product/1");
            Assert.True(res.IsSuccessStatusCode);
        }
        //[Fact]
        //public async Task PutShopItem_ConnectionTests()
        //{
        //    var client = factory.CreateDefaultClient();
        //    var res = await client.PutAsync("Api/Admin/Shop", httpContent);
        //    Assert.True(res.IsSuccessStatusCode);
        //}
        //[Fact]
        //public async Task PutProductItem_ConnectionTests()
        //{
        //    var client = factory.CreateDefaultClient();
        //    var res = await client.PutAsync("Api/Admin/Product", httpContent);
        //    Assert.True(res.IsSuccessStatusCode);
        //}
        //[Fact]
        //public async Task AddShopItem_ConnectionTests()
        //{
        //    var client = factory.CreateDefaultClient();
        //    var res = await client.PostAsync("Api/Admin/Shop");
        //    Assert.True(res.IsSuccessStatusCode);
        //}
        //[Fact]
        //public async Task AddProductItem_ConnectionTests()
        //{
        //    var client = factory.CreateDefaultClient();
        //    var res = await client.PostAsync("Api/Admin/Product");
        //    Assert.True(res.IsSuccessStatusCode);
        //}



    }
}
using AdminService;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace WebApiGeneral.IntegrationTests
{
    public class BehaviorAllIntegrationTests : BaseIntegrationTests
    {
        [Fact]
        public async Task Admin_CRUD_ForShopAndProduct()
        {
            var productMessage = new ProductDtoMessage { Id = 1, Name = "Milk", Price = 100, ShopId = 1 };
            var productByteContent = ConverToContent(productMessage);
            var shopMessage = new ShopDtoMessage { Id = 1, Name = "Silpo"};
            var shopByteContent = ConverToContent(shopMessage);

            var client = factory.CreateDefaultClient();
            var resPostShopAdd = await client.PostAsync("Api/Admin/Shop", shopByteContent);
            var resPostProductAdd = await client.PostAsync("Api/Admin/Product", productByteContent);
            Assert.True(resPostShopAdd.IsSuccessStatusCode);
            Assert.True(resPostProductAdd.IsSuccessStatusCode);

            var resGetShops = await client.GetAsync("Api/Admin/Shops");
            var resGetProducts = await client.GetAsync("Api/Admin/Products");
            Assert.True(resGetShops.IsSuccessStatusCode);
            Assert.True(resGetProducts.IsSuccessStatusCode);

            shopMessage.Name = "Metro";
            productMessage.Name = "MilkiWay";
            productByteContent = ConverToContent(productMessage);
            shopByteContent = ConverToContent(shopMessage);

            var resPutShopAdd = await client.PutAsync("Api/Admin/Shop", shopByteContent);
            var resPutProductAdd = await client.PutAsync("Api/Admin/Product", productByteContent);
            Assert.True(resPutShopAdd.IsSuccessStatusCode);
            Assert.True(resPutProductAdd.IsSuccessStatusCode);

            var resGetShop = await client.GetAsync("Api/Admin/Shop/1");
            var resGetProduct = await client.GetAsync("Api/Admin/Product/1");
            Assert.True(resGetShop.IsSuccessStatusCode);
            Assert.True(resGetProduct.IsSuccessStatusCode);

            var resDeleteShop = await client.DeleteAsync("Api/Admin/Shop/1");
            var resDeleteProduct = await client.DeleteAsync("Api/Admin/Product/1");
            Assert.True(resDeleteShop.IsSuccessStatusCode);
            Assert.True(resDeleteProduct.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Wife_AllGets()
        {
            var productMessage = new ProductDtoMessage { Id = 1, Name = "Milk", Price = 100, ShopId = 1 };
            var productByteContent = ConverToContent(productMessage);
            var shopMessage = new ShopDtoMessage { Id = 1, Name = "Silpo" };
            var shopByteContent = ConverToContent(shopMessage);

            var client = factory.CreateDefaultClient();
            var resPostShopAdd = await client.PostAsync("Api/Admin/Shop", shopByteContent);
            var resPostProductAdd = await client.PostAsync("Api/Admin/Product", productByteContent);
            Assert.True(resPostShopAdd.IsSuccessStatusCode);
            Assert.True(resPostProductAdd.IsSuccessStatusCode);

            var id = 1;
            var idByteContent = ConverToContent(id);
            var resPostWantedProductAdd0 = await client.PostAsync("Api/Wife/Product/1", idByteContent);
            var resPostWantedProductAdd1 = await client.PostAsync("Api/Wife/Product/1", idByteContent);
            Assert.True(resPostWantedProductAdd0.IsSuccessStatusCode);
            Assert.True(resPostWantedProductAdd1.IsSuccessStatusCode);

            var resGetProducts = await client.GetAsync("Api/Wife/Products");
            var resGetProduct = await client.GetAsync("Api/Wife/Product/1");
            var resGetProductsTotalAmount = await client.GetAsync("Api/Wife/Products/TotalAmount");
            Assert.True(resGetProducts.IsSuccessStatusCode);
            Assert.True(resGetProduct.IsSuccessStatusCode);
            Assert.True(resGetProductsTotalAmount.IsSuccessStatusCode);

            var resDeleteShop = await client.DeleteAsync("Api/Wife/Product/1");
            var resDeleteProduct = await client.DeleteAsync("Api/Wife/Products");
            Assert.True(resDeleteShop.IsSuccessStatusCode);
            Assert.True(resDeleteProduct.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Husband_AllGets ()
        {
            var productMessage = new ProductDtoMessage { Id = 1, Name = "Milk", Price = 100, ShopId = 1 };
            var productByteContent = ConverToContent(productMessage);
            var shopMessage = new ShopDtoMessage { Id = 1, Name = "Silpo" };
            var shopByteContent = ConverToContent(shopMessage);

            var client = factory.CreateDefaultClient();
            var resPostShopAdd = await client.PostAsync("Api/Admin/Shop", shopByteContent);
            var resPostProductAdd = await client.PostAsync("Api/Admin/Product", productByteContent);
            Assert.True(resPostShopAdd.IsSuccessStatusCode);
            Assert.True(resPostProductAdd.IsSuccessStatusCode);

            var id = 1;
            var idByteContent = ConverToContent(id);
            var resPostWantedProductAdd = await client.PostAsync("Api/Wife/Product/1", idByteContent);
            Assert.True(resPostWantedProductAdd.IsSuccessStatusCode);

            var resGetShops = await client.GetAsync("Api/Husband/Shops");
            var resGetProducts = await client.GetAsync("Api/Husband/Products");
            var resGetProductsInShop = await client.GetAsync("Api/Husband/ProductsInShop/1");
            Assert.True(resGetShops.IsSuccessStatusCode);
            Assert.True(resGetProducts.IsSuccessStatusCode); 
            Assert.True(resGetProductsInShop.IsSuccessStatusCode);

        }


        private ByteArrayContent ConverToContent(ShopDtoMessage message)
        {
            var myContent = JsonConvert.SerializeObject(message);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
        private ByteArrayContent ConverToContent(ProductDtoMessage message)
        {
            var myContent = JsonConvert.SerializeObject(message);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
        private ByteArrayContent ConverToContent(int id)
        {
            var myContent = JsonConvert.SerializeObject(id);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}

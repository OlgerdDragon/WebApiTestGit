using Grpc.Core;
using System.Threading.Tasks;

namespace AdminService.Services
{
    public interface IAdminGreeterService
    {
        public Task<GetProductsReply> GetProducts(UserLoginRequest request, ServerCallContext context);
        public Task<GetShopsReply> GetShops(UserLoginRequest request, ServerCallContext context);
        public Task<ShopReply> UpdateShop(ShopRequest request, ServerCallContext context);
        public Task<ProductReply> UpdateProduct(ProductRequest request, ServerCallContext context);
        public Task<ShopReply> GetShop(ItemRequest request, ServerCallContext context);
        public Task<ProductReply> GetProduct(ItemRequest request, ServerCallContext context);
        public Task<BoolReply> RemoveProduct(ItemRequest request, ServerCallContext context);       
        public Task<BoolReply> RemoveShop(ItemRequest request, ServerCallContext context);       
        public Task<BoolReply> AddProduct(ProductRequest request, ServerCallContext context);
        public Task<BoolReply> AddShop(ShopRequest request, ServerCallContext context);
        public Task<GetProductsInShopReply> GetProductsInShop(GetProductsInShopRequest request, ServerCallContext context);
        public Task<GetShopsForVisitReply> GetShopsForVisit(GetShopsForVisitRequest request, ServerCallContext context);
    }
}

using Grpc.Core;
using System.Threading.Tasks;

namespace HusbandGrpcService.Services
{
    public interface IHusbandGreeterService
    {
        public Task<GetWantedProductsReply> GetWantedProducts(UserLoginRequest request, ServerCallContext context);
        public Task<GetShopsForVisitReply> GetShopsForVisit(UserLoginRequest request, ServerCallContext context);
        public Task<GetProductsInShopReply> GetProductsInShop(GetProductsInShopRequest request, ServerCallContext context);
        
    }
}

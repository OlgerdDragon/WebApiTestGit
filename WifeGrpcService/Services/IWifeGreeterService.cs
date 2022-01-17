using Grpc.Core;
using System.Threading.Tasks;

namespace WifeGrpcService.Services
{
    public interface IWifeGreeterService
    {
        public Task<GetWantedProductsReply> GetWantedProducts(UserLoginRequest request, ServerCallContext context);
        public Task<GetTotalAmountWantedProductsReply> GetTotalAmountWantedProducts(UserLoginRequest request, ServerCallContext context);
        public Task<WantedProductReply> AddWantedProduct(ItemRequest request, ServerCallContext context);
        public Task<WantedProductReply> GetWantedProductItem(ItemRequest request, ServerCallContext context);
        public Task<BoolReply> RemoveWantedProduct(ItemRequest request, ServerCallContext context);
        public Task<BoolReply> RemoveAllWantedProducts(UserLoginRequest request, ServerCallContext context);
    }
}


using WifeGrpcService;

namespace WebApiGeneralGrpc.Services.WifeService
{
    public interface IWifeServiceFactory
    {
        WifeGreeter.WifeGreeterClient GetGrpcClient();
    }
}
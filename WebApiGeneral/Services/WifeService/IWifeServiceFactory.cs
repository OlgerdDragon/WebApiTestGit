
using WifeService;

namespace WebApiGeneral.Services.WifeService
{
    public interface IWifeServiceFactory
    {
        WifeGreeter.WifeGreeterClient GetGrpcClient();
    }
}
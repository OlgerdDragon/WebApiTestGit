
using HusbandGrpcService;

namespace WebApiGeneralGrpc.Services.HusbandService
{
    public interface IHusbandServiceFactory
    {
        HusbandGreeter.HusbandGreeterClient GetGrpcClient();
    }
}

using HusbandGrpcService;

namespace WifeGrpcService.Services.HusbandService
{
    public interface IHusbandServiceFactory
    {
        HusbandGreeter.HusbandGreeterClient GetGrpcClient();
    }
}
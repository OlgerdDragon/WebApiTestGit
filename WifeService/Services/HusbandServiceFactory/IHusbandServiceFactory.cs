
using HusbandService;

namespace WifeService.Services.HusbandServiceFactory
{
    public interface IHusbandServiceFactory
    {
        HusbandGreeter.HusbandGreeterClient GetGrpcClient();
    }
}
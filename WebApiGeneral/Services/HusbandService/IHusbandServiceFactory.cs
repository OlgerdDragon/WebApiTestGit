
using HusbandService;

namespace WebApiGeneral.Services.HusbandService
{
    public interface IHusbandServiceFactory
    {
        HusbandGreeter.HusbandGreeterClient GetGrpcClient();
    }
}
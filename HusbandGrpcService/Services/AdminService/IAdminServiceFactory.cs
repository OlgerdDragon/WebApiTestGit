
using AdminGrpcService;

namespace HusbandGrpcService.Services.AdminService
{
    public interface IAdminServiceFactory
    {
        AdminGreeter.AdminGreeterClient GetGrpcClient();
    }
}
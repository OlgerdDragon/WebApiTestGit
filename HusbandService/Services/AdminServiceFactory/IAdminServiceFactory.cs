
using AdminService;

namespace HusbandService.Services.AdminServiceFactory
{
    public interface IAdminServiceFactory
    {
        AdminGreeter.AdminGreeterClient GetGrpcClient();
    }
}
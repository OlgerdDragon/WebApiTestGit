
using AdminService;

namespace WebApiGeneral.Services.AdminService
{
    public interface IAdminServiceFactory
    {
        AdminGreeter.AdminGreeterClient GetGrpcClient();
    }
}
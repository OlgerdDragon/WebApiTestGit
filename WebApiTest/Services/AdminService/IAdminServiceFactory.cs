
using AdminGrpcService;

namespace WebApiGeneralGrpc.Services.AdminService
{
    public interface IAdminServiceFactory
    {
        AdminGreeter.AdminGreeterClient GetGrpcClient();
    }
}
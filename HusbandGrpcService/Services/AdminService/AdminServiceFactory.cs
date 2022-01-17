using AdminGrpcService;
using Grpc.Net.Client;

namespace HusbandGrpcService.Services.AdminService
{
    public class AdminServiceFactory : IAdminServiceFactory
    {
        public AdminGreeter.AdminGreeterClient GetGrpcClient()
        {
            var channels = GrpcChannel.ForAddress("https://localhost:5001");
            var adminServiceClient = new AdminGreeter.AdminGreeterClient(channels);
            return adminServiceClient;
        }
    }
}
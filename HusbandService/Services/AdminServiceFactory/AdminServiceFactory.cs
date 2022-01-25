using AdminService;
using Grpc.Net.Client;

namespace HusbandService.Services.AdminServiceFactory
{
    public class AdminServiceFactory : IAdminServiceFactory
    {
        public AdminGreeter.AdminGreeterClient GetGrpcClient()
        {
            var channels = GrpcChannel.ForAddress("https://localhost:5003");
            var adminServiceClient = new AdminGreeter.AdminGreeterClient(channels);
            return adminServiceClient;
        }
    }
}
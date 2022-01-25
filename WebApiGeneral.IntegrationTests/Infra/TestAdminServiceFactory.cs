using AdminService;
using Grpc.Net.Client;
using WebApiGeneral.Services.AdminService;

namespace WebApiGeneral.IntegrationTests.Infra
{
    public class TestAdminServiceFactory:IAdminServiceFactory
    {
        public AdminGreeter.AdminGreeterClient GetGrpcClient()
        {
            
            var  admin = new TestAdminWebApplicationFactory<AdminService.Startup>();
            
            var client = admin.CreateDefaultClient();
            var channels = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
            var adminServiceClient = new AdminGreeter.AdminGreeterClient(channels);
            return adminServiceClient;
        }
    }
   
}
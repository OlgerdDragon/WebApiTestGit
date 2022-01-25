using AdminService;
using Grpc.Net.Client;
using HusbandService.Services.AdminServiceFactory;

namespace WebApiGeneral.IntegrationTests.Infra
{
    public class TestAdminServiceFactoryInsideHusband : IAdminServiceFactory
    {
        public AdminGreeter.AdminGreeterClient GetGrpcClient()
        {

            var admin = new TestAdminWebApplicationFactory<AdminService.Startup>();

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
using AdminGrpcService;
using Grpc.Net.Client;
using HusbandGrpcService.Services.AdminService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    public class TestAdminServiceFactoryInsideHusband : IAdminServiceFactory
    {
        public AdminGreeter.AdminGreeterClient GetGrpcClient()
        {

            var admin = new TestAdminWebApplicationFactory<AdminGrpcService.Startup>();

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
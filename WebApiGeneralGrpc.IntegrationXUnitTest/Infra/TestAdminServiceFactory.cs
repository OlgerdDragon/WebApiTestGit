using AdminGrpcService;
using AdminGrpcService.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApiGeneralGrpc.Services.AdminService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    public class TestAdminServiceFactory:IAdminServiceFactory
    {
        public AdminGreeter.AdminGreeterClient GetGrpcClient()
        {
            var admin = new WebApplicationFactory<AdminGrpcService.Startup>();
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
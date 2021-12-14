using WifeGrpcService;
using AdminGrpcService.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApiGeneralGrpc.Services.WifeService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    public class TestWifeServiceFactory:IWifeServiceFactory
    {
        public WifeGreeter.WifeGreeterClient GetGrpcClient()
        {
            var wife = new WebApplicationFactory<WifeGrpcService.Startup>();
            var client = wife.CreateDefaultClient();
            var channels = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
            var wifeServiceClient = new WifeGreeter.WifeGreeterClient(channels);
            return wifeServiceClient;
        }
    }
}
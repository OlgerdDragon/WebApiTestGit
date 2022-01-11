using HusbandGrpcService;
using AdminGrpcService.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApiGeneralGrpc.Services.HusbandService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    public class TestHusbandServiceFactory:IHusbandServiceFactory
    {
        public HusbandGreeter.HusbandGreeterClient GetGrpcClient()
        {
            var husband = new TestHusbandWebApplicationFactory<HusbandGrpcService.Startup>();
            var client = husband.CreateDefaultClient();
            var channels = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
            var husbandServiceClient = new HusbandGreeter.HusbandGreeterClient (channels);
            return husbandServiceClient;
        }
    }
}
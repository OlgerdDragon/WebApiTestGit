using HusbandService;
using Grpc.Net.Client;
using WebApiGeneral.Services.HusbandService;

namespace WebApiGeneral.IntegrationTests.Infra
{
    public class TestHusbandServiceFactory : IHusbandServiceFactory
    {
        public HusbandGreeter.HusbandGreeterClient GetGrpcClient()
        {
            var husband = new TestHusbandWebApplicationFactory<HusbandService.Startup>();
            var client = husband.CreateDefaultClient();
            var channels = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
            var husbandServiceClient = new HusbandGreeter.HusbandGreeterClient(channels);
            return husbandServiceClient;
        }
    }
}
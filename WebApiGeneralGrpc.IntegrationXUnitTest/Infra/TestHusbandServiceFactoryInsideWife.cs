using HusbandGrpcService;
using Grpc.Net.Client;
using WifeGrpcService.Services.HusbandService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    public class TestHusbandServiceFactoryInsideWife: IHusbandServiceFactory
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
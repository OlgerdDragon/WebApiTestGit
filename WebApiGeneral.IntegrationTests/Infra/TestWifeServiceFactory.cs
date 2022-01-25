using WifeService;
using Grpc.Net.Client;
using WebApiGeneral.Services.WifeService;

namespace WebApiGeneral.IntegrationTests.Infra
{
    public class TestWifeServiceFactory:IWifeServiceFactory
    {
        public WifeGreeter.WifeGreeterClient GetGrpcClient()
        {
            var wife = new TestWifeWebApplicationFactory<WifeService.Startup>();
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
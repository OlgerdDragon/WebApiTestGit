using WifeService;
using Grpc.Net.Client;

namespace WebApiGeneral.Services.WifeService
{
    public class WifeServiceFactory : IWifeServiceFactory
    {
        public WifeGreeter.WifeGreeterClient GetGrpcClient()
        {
            var channels = GrpcChannel.ForAddress("https://localhost:5007");
            var wifeServiceClient = new WifeGreeter.WifeGreeterClient(channels);
            return wifeServiceClient;
        }
    }
}
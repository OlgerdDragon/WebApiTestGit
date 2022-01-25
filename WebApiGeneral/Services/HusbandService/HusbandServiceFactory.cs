using HusbandService;
using Grpc.Net.Client;

namespace WebApiGeneral.Services.HusbandService
{
    public class HusbandServiceFactory : IHusbandServiceFactory
    {
        public HusbandGreeter.HusbandGreeterClient GetGrpcClient()
        {
            var channels = GrpcChannel.ForAddress("https://localhost:5005");
            var husbandServiceClient = new HusbandGreeter.HusbandGreeterClient(channels);
            return husbandServiceClient;
        }
    }
}
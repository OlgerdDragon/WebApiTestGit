using WebApiGeneralGrpc;
using WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class BaseIntegrationTests
    {
        public readonly TestWebApplicationFactory<Startup> factory;
        public BaseIntegrationTests()
        {
            factory = new();
        }
    }
}

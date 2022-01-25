using WebApiGeneral.IntegrationTests.Infra;

namespace WebApiGeneral.IntegrationTests
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

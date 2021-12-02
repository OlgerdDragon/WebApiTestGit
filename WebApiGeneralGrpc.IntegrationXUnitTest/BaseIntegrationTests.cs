using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiGeneralGrpc;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class BaseIntegrationTests
    {
        public readonly WebApplicationFactory<Startup> webHost;
        public BaseIntegrationTests()
        {
            webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });

        }

    }
}

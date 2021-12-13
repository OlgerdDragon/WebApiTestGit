using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

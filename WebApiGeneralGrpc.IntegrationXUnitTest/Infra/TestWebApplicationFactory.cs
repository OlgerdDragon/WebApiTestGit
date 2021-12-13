using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using WebApiGeneralGrpc.Services.AdminService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    public class TestWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        
        internal ConcurrentDictionary<string, HttpClient> HttpClients { get; } = new();
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Tests");
            
            /*
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IHttpClientFactory>(
                    new BaseIntegrationTestsV3.CustomHttpClientFactory(HttpClients));
            });
            */

            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAdminServiceFactory));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
            
                services.AddScoped<IAdminServiceFactory, TestAdminServiceFactory>();
            });
        }

    }
}
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TownContextForWebService;
using WebApiGeneralGrpc.Services.AdminService;
using WebApiGeneralGrpc.Services.HusbandService;
using WebApiGeneralGrpc.Services.WifeService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    
    public class TestWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
       
        internal ConcurrentDictionary<string, HttpClient> HttpClients { get; } = new();
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Tests");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault
                   (d => d.ServiceType == typeof(DbContextOptions<TownContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                
                services.AddDbContext<TownContext>
                  ((_, context) => context.UseInMemoryDatabase("InMemoryDbForTesting"));

                var serviceProvider = services.BuildServiceProvider();


                using var scope = serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<TownContext>();

                db.Database.EnsureCreated();


            });

            builder.ConfigureTestServices(services =>
            {
                var descriptorAdmin = services.SingleOrDefault(d => d.ServiceType == typeof(IAdminServiceFactory));
                if (descriptorAdmin != null)
                {
                    services.Remove(descriptorAdmin);
                }

                var descriptorHusband = services.SingleOrDefault(d => d.ServiceType == typeof(IHusbandServiceFactory));
                if (descriptorHusband != null)
                {
                    services.Remove(descriptorHusband);
                }

                var descriptorWife = services.SingleOrDefault(d => d.ServiceType == typeof(IWifeServiceFactory));
                if (descriptorWife != null)
                {
                    services.Remove(descriptorHusband);
                }
             
                services.AddScoped<IAdminServiceFactory, TestAdminServiceFactory>();
                services.AddScoped<IHusbandServiceFactory, TestHusbandServiceFactory>();
                services.AddScoped<IWifeServiceFactory, TestWifeServiceFactory>();
            });
        }

    }
}
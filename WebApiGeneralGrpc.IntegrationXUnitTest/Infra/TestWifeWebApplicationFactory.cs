using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TownContextForWebService;
using WifeGrpcService.Services.HusbandService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    public class TestWifeWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
       
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
                  ((_, context) => context.UseInMemoryDatabase("InMemoryDbForTestingWife"));

                var serviceProvider = services.BuildServiceProvider();


                using var scope = serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<TownContext>();

                db.Database.EnsureCreated();


            });

            builder.ConfigureTestServices(services =>
            {
                var descriptorHusband = services.SingleOrDefault(d => d.ServiceType == typeof(IHusbandServiceFactory));
                if (descriptorHusband != null)
                {
                    services.Remove(descriptorHusband);
                }

                services.AddScoped<IHusbandServiceFactory, TestHusbandServiceFactoryInsideWife>();
            });
        }

    }
}
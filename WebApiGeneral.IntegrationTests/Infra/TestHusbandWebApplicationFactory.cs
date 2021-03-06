using System.Linq;
using DbApiContextForService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using HusbandService.Services.AdminServiceFactory;

namespace WebApiGeneral.IntegrationTests.Infra
{
    public class TestHusbandWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
       
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Tests");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault
                   (d => d.ServiceType == typeof(DbContextOptions<DbApiContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DbApiContext>
                  ((_, context) => context.UseInMemoryDatabase("InMemoryDbForTestingHusband"));

                var serviceProvider = services.BuildServiceProvider();


                using var scope = serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<DbApiContext>();

                db.Database.EnsureCreated();


            });

            builder.ConfigureTestServices(services =>
            {
                var descriptorAdmin = services.SingleOrDefault(d => d.ServiceType == typeof(IAdminServiceFactory));
                if (descriptorAdmin != null)
                {
                    services.Remove(descriptorAdmin);
                }

                services.AddScoped<IAdminServiceFactory, TestAdminServiceFactoryInsideHusband>();
            });

        }
    }
}
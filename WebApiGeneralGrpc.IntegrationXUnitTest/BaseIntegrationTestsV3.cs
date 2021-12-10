using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiGeneralGrpc.Data;
using System.Net.Http.Json;
using WebApiGeneralGrpc.Models;
using Moq;
using Microsoft.Extensions.Logging;
using AdminGrpcService.Services;
using WebApiGeneralGrpc.Controllers;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class BaseIntegrationTestsV3
    {
        protected readonly HttpClient globalTestClient;
        protected readonly HttpClient adminTestClient;
        protected readonly IServiceProvider adminTestService;

        private Mock<AdminGrpcService.Data.TownContext> _adminContext = new Mock<AdminGrpcService.Data.TownContext>(new DbContextOptions<AdminGrpcService.Data.TownContext>());
        private Mock<ILogger<AdminGrpcService.Services.AdminGreeterService>> _adminLogger = new Mock<ILogger<AdminGrpcService.Services.AdminGreeterService>>();

        protected BaseIntegrationTestsV3()
        {

            var appAdminFactory = new WebApplicationFactory<AdminGrpcService.Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContext));
                        services.AddDbContext<DbContext>(options => { options.UseInMemoryDatabase("TestDb"); });

                        var serviceProvider = services.BuildServiceProvider();
                        var scope = serviceProvider.CreateScope();
                        var scopedServices = scope.ServiceProvider;

                       
                    });
                });

            adminTestClient = appAdminFactory.CreateClient();
            adminTestService = appAdminFactory.Services;
            
            var appGlobalFactory = new WebApplicationFactory<WebApiGeneralGrpc.Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddScoped<AdminController>();


                        services.RemoveAll(typeof(DbContext));
                        services.AddDbContext<DbContext>(options => { options.UseInMemoryDatabase("TestDb"); });

                        services.AddControllers();


                    });

                });
            
            globalTestClient = appGlobalFactory.CreateClient();
            
            var server = appAdminFactory.Server;
            
        }



        internal class CustomHttpClientFactory : IHttpClientFactory
        {
            private IReadOnlyDictionary<string,HttpClient> HttpClients { get; }
            public CustomHttpClientFactory(IReadOnlyDictionary<string, HttpClient> httpClients)
            {
                HttpClients = httpClients;
            }

            public HttpClient CreateClient(string name) =>
                HttpClients.GetValueOrDefault(name)
                ?? HttpClients.GetValueOrDefault("default")
                ?? throw new InvalidOperationException(
                    $"HTTP client is not found for client with name {name}");
        }


        
    }
}

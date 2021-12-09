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

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class BaseIntegrationTestsV3
    {
        protected readonly HttpClient globalTestClient;
        protected readonly HttpClient adminTestClient;


        protected BaseIntegrationTestsV3()
        {

            var appAdminFactory = new WebApplicationFactory<AdminGrpcService.Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContext));
                        services.AddDbContext<DbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                    });
                });

            adminTestClient = appAdminFactory.CreateClient();

            var appGlobalFactory = new WebApplicationFactory<WebApiGeneralGrpc.Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContext));
                        services.AddDbContext<DbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                    });

                });

            globalTestClient = appGlobalFactory.CreateClient();

        }

        
    }
}

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using AdminGrpcService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using NUnit.Framework;
using WebApiGeneralGrpc.Data;
using WebApiGeneralGrpc.Models;
using WebApiGeneralGrpc.Services.AdminService;
using WebApiGeneralGrpc.Services.HusbandService;
using WebApiGeneralGrpc.Services.WifeService;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest.Infra
{
    public class TestWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        //private static DbContextOptions<TownContext> dbContextOptions = new DbContextOptionsBuilder<TownContext>()
        //    .UseInMemoryDatabase(databaseName: "TestDB")
        //    .Options;

        //TownContext context;
        //[OneTimeSetUp]
        //public void Setup()
        //{
        //    context = new TownContext(dbContextOptions);
        //    context.Database.EnsureCreated();

        //}
       
        internal ConcurrentDictionary<string, HttpClient> HttpClients { get; } = new();
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Tests");

            //builder.ConfigureServices(services =>
            //{
            //    var descriptor = services.SingleOrDefault
            //       (d => d.ServiceType == typeof(DbContextOptions<TownContext>));

            //    if (descriptor != null)
            //    {
            //        services.Remove(descriptor);
            //    }

            //    services.AddDbContext<TownContext>
            //      ((_, context) => context.UseInMemoryDatabase("InMemoryDbForTesting"));

            //    var serviceProvider = services.BuildServiceProvider();


            //    using var scope = serviceProvider.CreateScope();

            //    var db = scope.ServiceProvider.GetRequiredService<TownContext>();
            //    var logger = scope.ServiceProvider.GetRequiredService
            //                 <ILogger<TestWebApplicationFactory<TStartup>>>();

            //    db.Database.EnsureCreated();


            //});


            builder.ConfigureServices(services =>
            {
                //services.RemoveAll(typeof(DbContextOptions<TownContext>));
                //services.RemoveAll(typeof(TownContext));
                //services.RemoveAll(typeof(IServiceCollection));
                //services.AddDbContext<TownContext>(options => { options.UseInMemoryDatabase("TestDb1"); });

                services.RemoveAll(typeof(IServiceProvider));
                services.AddSingleton((serviceProvider) =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<TownContext>().UseInMemoryDatabase("orders1");
                    return optionsBuilder.Options;
                });
                services.AddScoped<TownContext>();
                services.AddMvc();
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
                //services.RemoveAll(typeof(DbContextOptions));
                //services.AddDbContext<TownContext>(options => { options.UseInMemoryDatabase("TestDb"); });



                //var descriptorDb = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TownContext>));
                //if (descriptorDb != null)
                //    services.Remove(descriptorDb);

                //services.AddDbContext<TownContext>(options =>
                //    options.UseInMemoryDatabase("TestDB"));




                //var descriptor = services.SingleOrDefault
                //   (d => d.ServiceType == typeof(DbContextOptions<TownContext>));

                //if (descriptor != null)
                //{
                //    services.Remove(descriptor);
                //}

                //services.AddDbContext<TownContext>
                //  ((_, context) => context.UseInMemoryDatabase(Configuration));







                //services.RemoveAll(typeof(TownContext));
                //var options = new DbContextOptionsBuilder<TownContext>()
                //                  .UseInMemoryDatabase("TestDb2")
                //                  .Options;
                //var context = new TownContext(options);
                //context.Database.EnsureCreated();
                //services.AddDbContext<TownContext>(options);





                //services.RemoveAll(typeof(DbContextOptions<TownContext>));
                //services.RemoveAll(typeof(TownContext));
                //services.RemoveAll(typeof(IServiceCollection));
                //services.AddDbContext<TownContext>(options => { options.UseInMemoryDatabase("TestDb"); });

                services.RemoveAll(typeof(IServiceProvider));
                //services.AddSingleton(typeof(IRepository), typeof(InMemoryRepository));
                services.AddSingleton((serviceProvider) =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<TownContext>().UseInMemoryDatabase("orders");
                    return optionsBuilder.Options;
                });


                //var descriptorBd = services.SingleOrDefault(
                //d => d.ServiceType ==
                //    typeof(DbContextOptions<TownContext>));
                //if (descriptorBd != null)
                //    services.Remove(descriptorBd);
                //services.AddDbContext<TownContext>(options =>
                //{
                //    options.UseInMemoryDatabase("InMemoryEmployeeTest");
                //});
                //var sp = services.BuildServiceProvider();
                //using (var scope = sp.CreateScope())
                //using (var appContext = scope.ServiceProvider.GetRequiredService<TownContext>())
                //{
                //    try
                //    {
                //        appContext.Database.EnsureCreated();
                //    }
                //    catch (Exception ex)
                //    {
                //        throw;
                //    }
                //}


                services.AddScoped<IAdminServiceFactory, TestAdminServiceFactory>();
                services.AddScoped<IHusbandServiceFactory, TestHusbandServiceFactory>();
                services.AddScoped<IWifeServiceFactory, TestWifeServiceFactory>();
            });
        }

    }
}
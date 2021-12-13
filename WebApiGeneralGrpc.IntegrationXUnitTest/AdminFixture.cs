using AdminGrpcService;
using AdminGrpcService.Models.Dto;
using AdminGrpcService.Services;
using Castle.Core.Logging;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Collections;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public sealed class AdminFixture : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public GrpcChannel GrpcChannel { get; }

        public AdminFixture()
        {
            _factory = new WebApplicationFactory<Startup>();
            var client = _factory.CreateDefaultClient(new ResponseVersionHandler());
            GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
        }

        public void Dispose()
        {
            _factory.Dispose();
        }

        private class ResponseVersionHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var response = await base.SendAsync(request, cancellationToken);
                response.Version = request.Version;
                return response;
            }
        }
    }

    public class LazyCounterServiceShould
    {
        private string userLogin = "adminUnitTest";
        private Mock<ServerCallContext> serverCallContext = new Mock<ServerCallContext>();
        private readonly IAdminGreeterService _clientService;
        private AdminGreeter.AdminGreeterClient adminServiceClient;

        public LazyCounterServiceShould(AdminFixture testServerFixture)
        {
            var channel = testServerFixture.GrpcChannel;
            _clientService = channel.CreateGrpcService<IAdminGreeterService>();
        }
        

        [Fact]
        public async void FastCountFromLowToHigh()
        {
            // arrange
            var request = new UserLoginRequest() { UserLogin = userLogin };

            // ac
            var result = await _clientService.GetShops(request, serverCallContext.Object);

            // assert
            var resultReplyList = result.Element;
            var resultList = resultReplyList.ShopDtoMessage.ToList();
            //resultList.ShouldNotBeNull();
            //resultList.Count().ShouldBe(10);
            //resultList.First().Value.ShouldBe(1);
            //resultList.Last().Value.ShouldBe(10);
        }

        [Fact]
        public async Task SlowCountFromLowToHighAsync()
        {

            // arrange
            var timer = new Stopwatch();
            var request = new UserLoginRequest() { UserLogin = userLogin };

            // act
            timer.Start();
            var result = await _clientService.GetShops(request, serverCallContext.Object);

            // assert
            

            timer.Stop();
            //counter.ShouldBe(6);
            //timer.Elapsed.ShouldBeGreaterThan(TimeSpan.FromSeconds(5));
        }
    }




    //public class GrpcTestFixture<TStartup> : IDisposable where TStartup : class
    //{
    //    private readonly TestServer _server;
    //    private readonly IHost _host;

    //    public LoggerFactory LoggerFactory { get; }
    //    public HttpClient Client { get; }
    //    public GrpcTestFixture() : this(null) { }

    //    public GrpcTestFixture(Action<IServiceCollection>? initialConfigureServices)
    //    {
    //        var builder = new HostBuilder()
    //           .ConfigureServices(services =>
    //           {
    //               initialConfigureServices?.Invoke(services);
    //               services.AddSingleton<ILoggerFactory>(LoggerFactory);
    //           })
    //           .ConfigureWebHostDefaults(webHost =>
    //           {
    //               webHost
    //                   .UseTestServer()
    //                   .UseStartup<TStartup>();
    //           });
    //        _host = builder.Start();
    //        _server = _host.GetTestServer();

    //        var handler = _server.CreateHandler();

    //        var client = new HttpClient(handler);
    //        client.BaseAddress = new Uri("http://localhost");

    //        Client = client;
    //    }

        

    //    public void Dispose()
    //    {
    //        Client.Dispose();
    //        _host.Dispose();
    //        _server.Dispose();
    //    }

    //}
}

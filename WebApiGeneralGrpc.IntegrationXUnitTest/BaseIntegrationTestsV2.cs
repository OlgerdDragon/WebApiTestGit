using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiGeneralGrpc;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiGeneralGrpc.Data;
using System.Net.Http.Json;
using WebApiGeneralGrpc.Models;

namespace WebApiGeneralGrpcTests.IntegrationXUnitTest
{
    public class BaseIntegrationTestsV2
    {
        protected readonly HttpClient TestClient;

        protected BaseIntegrationTestsV2()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContext));
                        services.AddDbContext<DbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                    });
                });

            TestClient = appFactory.CreateClient();
        }

        //protected async Task AuthenticateAsync()
        //{
        //    TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        //}

        //protected async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
        //{
        //    var response = await TestClient.PostAsJsonAsync(ApiRoutes.Posts.Create, request);
        //    return await response.Content.ReadAsAsync<PostResponse>();
        //}

        //private async Task<string> GetJwtAsync()
        //{
        //    var response = await TestClient.PostAsJsonAsync(Persons, new Person
        //    {
        //        Login = "admin@gmail.com",
        //        Password = "1"
        //    });

        //    var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
        //    return registrationResponse.Token;
        //}
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Data;
using WebApiTest.Services.AdminService;
using WebApiTest.Services.HusbandService;
using WebApiTest.Services.WifeService;
using WebApiTest.Services.AccountService;
using WebApiTest.Services.UtilsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace WebApiTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IHusbandService, HusbandService>();
            services.AddScoped<IWifeService, WifeService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUtilsService, UtilsService>();
            services.AddControllers();
            var connectionString = Configuration.GetSection("ConnectionStrings")?["DbConnection"] ?? "";

            services.AddDbContext<TownContext>(options => options
                .UseSqlServer(connectionString, sqlOptions => { sqlOptions.EnableRetryOnFailure(); }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiTest", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,
                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });
            services.AddControllersWithViews();
        }
        private void LoggingConfiguration()
        {
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = LogEventLevel.Verbose;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.ControlledBy(levelSwitch)
                .CreateLogger();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            LoggingConfiguration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

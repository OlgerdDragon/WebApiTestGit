using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApiGeneral.Services.AdminService;
using WebApiGeneral.Services.HusbandService;
using WebApiGeneral.Services.AccountService;
using WebApiGeneral.Services.UtilsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApiGeneral.Health;
using DbApiContextForService;


namespace WebApiGeneral
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
            services.AddScoped<IAdminServiceFactory, AdminServiceFactory>();
            services.AddScoped<IHusbandServiceFactory, HusbandServiceFactory>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUtilsService, UtilsService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();
            var connectionString = Configuration.GetSection("ConnectionStrings")?["DbConnection"] ?? "";

            services.AddDbContext<DbApiContext>(options => options
                .UseSqlServer(connectionString, sqlOptions => { sqlOptions.EnableRetryOnFailure(); }));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiGeneral", Version = "v1" });
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
            services.AddHttpContextAccessor();
            
            services.AddHealthChecks()
                .AddCheck("Service", () => HealthCheckResult.Healthy())
                .AddCheck<AdminHealthCheck>("Admin")
                .AddCheck<HusbandHealthCheck>("Husband")
                .AddCheck<WifeHealthCheck>("Wife"); 
            
               
        }
        private void LoggingConfiguration()
        {
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = LogEventLevel.Information;

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
                endpoints.MapHealthChecks("/health/Service",
                    new HealthCheckOptions
                    {
                        Predicate = registration => registration.Name.Equals("Service")
                    });
                endpoints.MapHealthChecks("/health/Admin",
                    new HealthCheckOptions
                    {
                        Predicate = registration => registration.Name.Equals("Admin")
                    });
                endpoints.MapHealthChecks("/health/Husband",
                   new HealthCheckOptions
                   {
                       Predicate = registration => registration.Name.Equals("Husband")
                   });
                endpoints.MapHealthChecks("/health/Wife",
                   new HealthCheckOptions
                   {
                       Predicate = registration => registration.Name.Equals("Wife")
                   });
            });
        }
    }
}

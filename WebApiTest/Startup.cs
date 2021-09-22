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

namespace WebApiTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IHusbandService, HusbandService>();
            services.AddScoped<IWifeService, WifeService>();
            services.AddControllers();

            var connectionString = Configuration.GetSection("ConnectionStrings")?["DbConnection"] ?? "";
            
            //services.AddDbContext<TodoContext>(options => options
            //    .UseSqlServer(connectionString, sqlOptions => { sqlOptions.EnableRetryOnFailure(); }));

            services.AddDbContext<TowerContext>(options => options
                .UseSqlServer(connectionString, sqlOptions => { sqlOptions.EnableRetryOnFailure(); }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiTest", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiTest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

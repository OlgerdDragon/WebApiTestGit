using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApiGeneral.Services.AdminService;

namespace WebApiGeneral.Health
{
    public class AdminHealthCheck:IHealthCheck
    {
        private readonly IAdminServiceFactory _adminServiceFactory;

        public AdminHealthCheck(IAdminServiceFactory adminServiceFactory)
        {
            _adminServiceFactory = adminServiceFactory;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var adminServiceClient = _adminServiceFactory.GetGrpcClient();

            try
            {
                var res = adminServiceClient.Health(new Empty());
                return Task.FromResult(res.Value ? HealthCheckResult.Healthy() : HealthCheckResult.Degraded());
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApiGeneral.Services.WifeService;

namespace WebApiGeneral.Health
{
    public class WifeHealthCheck:IHealthCheck
    {
        private readonly IWifeServiceFactory _wifeServiceFactory;

        public WifeHealthCheck(IWifeServiceFactory wifeServiceFactory)
        {
            _wifeServiceFactory = wifeServiceFactory;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var wifeServiceClient = _wifeServiceFactory.GetGrpcClient();

            try
            {
                var res = wifeServiceClient.Health(new Empty());
                return Task.FromResult(res.Value ? HealthCheckResult.Healthy() : HealthCheckResult.Degraded());
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
            }
        }
    }
}
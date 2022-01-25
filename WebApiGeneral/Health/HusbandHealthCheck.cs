using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApiGeneral.Services.HusbandService;

namespace WebApiGeneral.Health
{
    public class HusbandHealthCheck:IHealthCheck
    {
        private readonly IHusbandServiceFactory _husbandServiceFactory;

        public HusbandHealthCheck(IHusbandServiceFactory husbandServiceFactory)
        {
            _husbandServiceFactory = husbandServiceFactory;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var husbandServiceClient = _husbandServiceFactory.GetGrpcClient();

            try
            {
                var res = husbandServiceClient.Health(new Empty());
                return Task.FromResult(res.Value ? HealthCheckResult.Healthy() : HealthCheckResult.Degraded());
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
            }
        }
    }
}
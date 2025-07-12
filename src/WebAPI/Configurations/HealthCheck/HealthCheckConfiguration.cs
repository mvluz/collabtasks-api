using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAPI.Configurations.HealthCheck
{
    public static class HealthCheckConfiguration
    {
        public static void ConfigureHealthChecks(WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddCheck("API Running", () => HealthCheckResult.Healthy("API is healthy"));
        }
    }

}

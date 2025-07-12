using Serilog;

namespace WebAPI.Configurations.Logging
{
    public static class SerilogConfiguration
    {
        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            // Serilog configuration to send logs to Fluentd
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Fluentd("tcp://fluentd-host:24224") // Replace with your Fluentd address
                .CreateLogger();

            // Add Serilog to the ASP.NET Core logging pipeline
            builder.Host.UseSerilog();
        }
    }
}

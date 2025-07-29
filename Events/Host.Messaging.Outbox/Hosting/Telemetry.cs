using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Host.Messaging.Outbox.Hosting;

internal static class Telemetry
{
    internal static void ConfigureTelemetry(this IServiceCollection services, Settings settings)
    {
        services.AddOpenTelemetry().WithTracing(x =>
        {
            x.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService("service")
                    .AddTelemetrySdk()
                    .AddEnvironmentVariableDetector())
                .AddSource("MassTransit");
        });
    }
}
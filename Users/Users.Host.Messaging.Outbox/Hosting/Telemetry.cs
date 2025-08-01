using MassTransit.Logging;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Host.Messaging.Outbox.Hosting;

internal static class Telemetry
{
    internal static void ConfigureTelemetry(this IServiceCollection services, Settings settings, string applicationName)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddTelemetrySdk()
            .AddService(serviceName: applicationName)
            .AddEnvironmentVariableDetector();
        
        var otel = services.AddOpenTelemetry()
            .WithTracing(x =>
            {
                x.SetResourceBuilder(resourceBuilder)
                    .AddSource(DiagnosticHeaders.DefaultListenerName);
            });

        otel.UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri(settings.Telemetry.ConnectionString));
    }
}
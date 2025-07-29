using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetryBuilder = OpenTelemetry.OpenTelemetryBuilder;

namespace Api.Hosting;

internal static class Telemetry
{
    internal static OpenTelemetryBuilder WithTelemetry(this OpenTelemetryBuilder otel, Settings settings, string applicationName)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: applicationName);

        otel.WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(resourceBuilder);
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddMeter("Microsoft.AspNetCore.Hosting");
                metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
                metrics.AddConsoleExporter();
            })
            .WithTracing(tracing =>
            {
                tracing.SetResourceBuilder(resourceBuilder);
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddConsoleExporter();
            });

        otel.UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri(settings.Telemetry.ConnectionString));

        return otel;
    }
}
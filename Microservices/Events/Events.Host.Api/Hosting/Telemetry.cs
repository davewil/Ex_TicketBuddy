using MassTransit.Logging;
using MassTransit.Monitoring;
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
                metrics.AddAspNetCoreInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter(InstrumentationOptions.MeterName)
                    .AddConsoleExporter();
            })
            .WithTracing(tracing =>
            {
                tracing.SetResourceBuilder(resourceBuilder);
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter()
                    .AddSource(DiagnosticHeaders.DefaultListenerName);
            });

        otel.UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri(settings.Telemetry.ConnectionString));

        return otel;
    }
}
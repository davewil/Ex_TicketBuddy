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
            // .AddJaegerExporter(o =>
            // {
            //     o.AgentHost = HostMetadataCache.IsRunningInContainer ? "jaeger" : "localhost";
            //     o.AgentPort = 6831;
            //     o.MaxPayloadSizeInBytes = 4096;
            //     o.ExportProcessorType = ExportProcessorType.Batch;
            //     o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
            //     {
            //         MaxQueueSize = 2048,
            //         ScheduledDelayMilliseconds = 5000,
            //         ExporterTimeoutMilliseconds = 30000,
            //         MaxExportBatchSize = 512,
            //     };
            // });
        });
    }
}
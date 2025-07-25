

using Host.Messaging.Outbox;
using MassTransit;
using Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Persistence;

var settings = new Settings(new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build());
var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.AddDbContext<EventDbContext>(options =>
        {
            options.UseSqlServer(settings.Database.Connection, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        // services.AddOpenTelemetry().WithTracing(x =>
        // {
        //     x.SetResourceBuilder(ResourceBuilder.CreateDefault()
        //             .AddService("service")
        //             .AddTelemetrySdk()
        //             .AddEnvironmentVariableDetector())
        //         .AddSource("MassTransit");
        //     // .AddJaegerExporter(o =>
        //     // {
        //     //     o.AgentHost = HostMetadataCache.IsRunningInContainer ? "jaeger" : "localhost";
        //     //     o.AgentPort = 6831;
        //     //     o.MaxPayloadSizeInBytes = 4096;
        //     //     o.ExportProcessorType = ExportProcessorType.Batch;
        //     //     o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
        //     //     {
        //     //         MaxQueueSize = 2048,
        //     //         ScheduledDelayMilliseconds = 5000,
        //     //         ExporterTimeoutMilliseconds = 30000,
        //     //         MaxExportBatchSize = 512,
        //     //     };
        //     // });
        // });
        
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<EventDbContext>(o =>
            {
                o.UseSqlServer();
                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });
            x.AddConfigureEndpointsCallback((context, _, cfg) =>
            {
                cfg.UseEntityFrameworkOutbox<EventDbContext>(context);
            });

            x.SetKebabCaseEndpointNameFormatter();
            x.SetInMemorySagaRepositoryProvider();
            var applicationAssembly = MessagingAssembly.Assembly;
            x.AddConsumers(applicationAssembly);
            x.AddSagaStateMachines(applicationAssembly);
            x.AddSagas(applicationAssembly);
            x.AddActivities(applicationAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {                        
                cfg.Host(settings.RabbitMq.Host, settings.RabbitMq.VirtualHost, h =>
                {
                    h.Username(settings.RabbitMq.Username);
                    h.Password(settings.RabbitMq.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

await host.RunAsync();
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared.Hosting;
using TicketBuddy.ApiGateway;

var configuration = Configuration.Build();
var settings = new Settings(configuration);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromMemory(settings.Yarp.Routes, settings.Yarp.Clusters);

const string serviceName = "yarpProxy";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddOtlpExporter(o => o.Endpoint = settings.Telemetry.ConnectionString);
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource("Yarp.ReverseProxy") 
        .AddOtlpExporter(o => o.Endpoint = settings.Telemetry.ConnectionString)
    );

var app = builder.Build();

app.MapReverseProxy();
app.Run();
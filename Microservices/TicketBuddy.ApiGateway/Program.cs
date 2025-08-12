using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
    options.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

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
using System.Text.Json.Serialization;
using Application;
using Domain;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Persistence;
using WebHost;

namespace Api.Hosting;

internal sealed class EventApi(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration) : WebApi(webApplicationBuilder, configuration)
{
    private readonly Settings _settings = new(configuration);
    protected override string ApplicationName => nameof(EventApi);
    protected override string TelemetryConnectionString => _settings.Telemetry.ConnectionString;
    protected override List<JsonConverter> JsonConverters => Converters.GetConverters;

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddDbContext<EventDbContext>(options =>
        {
            options.UseSqlServer(_settings.Database.Connection, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });
        services.AddScoped<EventRepository>();
        services.AddScoped<EventService>();
    }
    
    protected override OpenTelemetryBuilder ConfigureTelemetry(WebApplicationBuilder builder)
    {
        var otel = base.ConfigureTelemetry(builder);
        otel.WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddMeter("Microsoft.AspNetCore.Hosting");
            metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
        });
        
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
        });
        
        Environment.SetEnvironmentVariable("OTEL_RESOURCE_ATTRIBUTES", $"service.name={ApplicationName}");
        otel.UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri(TelemetryConnectionString));
        
        return otel;
    }
}
using System.Text.Json.Serialization;
using Events.Application;
using Events.Domain;
using OpenTelemetry;
using Events.Persistence;
using WebHost;

namespace Api.Hosting;

internal sealed class EventsApi(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration) : WebApi(webApplicationBuilder, configuration)
{
    private readonly Settings _settings = new(configuration);
    protected override string ApplicationName => nameof(EventsApi);
    protected override string TelemetryConnectionString => _settings.Telemetry.ConnectionString;
    protected override List<JsonConverter> JsonConverters => Converters.GetConverters;

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.ConfigureDatabase(_settings);
        services.ConfigureMessaging(_settings);
        services.AddScoped<EventRepository>();
        services.AddScoped<EventService>();
        services.AddCorsAllowAll();
    }

    protected override OpenTelemetryBuilder ConfigureTelemetry(WebApplicationBuilder builder)
    {
        var otel = base.ConfigureTelemetry(builder);
        return otel.WithTelemetry(_settings, ApplicationName);
    }
    
    protected override void ConfigureApplication(WebApplication theApp)
    {
        base.ConfigureApplication(theApp);
        theApp.UseCorsAllowAll();
    }
}
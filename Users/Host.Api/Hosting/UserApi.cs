using System.Text.Json.Serialization;
using Application;
using Domain;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Persistence;
using Users.Domain;
using WebHost;

namespace Api.Hosting;

internal sealed class UserApi(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration) : WebApi(webApplicationBuilder, configuration)
{
    private readonly Settings _settings = new(configuration);
    protected override string ApplicationName => nameof(UserApi);
    protected override string TelemetryConnectionString => _settings.Telemetry.ConnectionString;
    protected override List<JsonConverter> JsonConverters => Converters.GetConverters;

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.ConfigureDatabase(_settings);
        services.ConfigureMessaging(_settings);
        services.AddScoped<UserRepository>();
        services.AddScoped<UserService>();
    }
    
    protected override OpenTelemetryBuilder ConfigureTelemetry(WebApplicationBuilder builder)
    {
        var otel = base.ConfigureTelemetry(builder);
        return otel.WithTelemetry(_settings, ApplicationName);
    }
}
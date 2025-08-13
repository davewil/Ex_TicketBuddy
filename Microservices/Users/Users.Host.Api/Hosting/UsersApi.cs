using System.Text.Json.Serialization;
using Users.Application;
using OpenTelemetry;
using Users.Persistence;
using Users.Domain;
using WebHost;

namespace Api.Hosting;

internal sealed class UsersApi(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration) : WebApi(webApplicationBuilder, configuration)
{
    private readonly Settings _settings = new(configuration);
    protected override string ApplicationName => nameof(UsersApi);
    protected override string TelemetryConnectionString => _settings.Telemetry.ConnectionString;
    protected override List<JsonConverter> JsonConverters => Converters.GetConverters;

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.ConfigureDatabase(_settings);
        services.ConfigureMessaging(_settings);
        services.AddScoped<UserRepository>();
        services.AddScoped<UserService>();
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
using System.Text.Json.Serialization;
using Application;
using Domain;
using Persistence;
using Repositories;
using WebHost;

namespace Api.Hosting;

internal sealed class EventApi(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration) : WebApi(webApplicationBuilder, configuration)
{
    private readonly Settings _settings = new(configuration);
    protected override string ApplicationName => nameof(EventApi);
    protected override string ApplicationInsightsConnectionString => _settings.ApplicationInsights.ConnectionString;
    protected override List<JsonConverter> JsonConverters => Converters.GetConverters;

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddScoped<Db>(_ => new Db(_settings.Database.Connection));
        services.AddScoped<EventRepository>();
        services.AddScoped<EventService>();
    }
}
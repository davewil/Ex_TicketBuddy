using Microsoft.Extensions.Configuration;
using Yarp.ReverseProxy.Configuration;

namespace TicketBuddy.ApiGateway;

internal class Settings
{
    private static IConfiguration Configuration = null!;
    internal TelemetrySettings Telemetry => new();
    internal YarpSettings Yarp => new();

   
    internal Settings(IConfiguration theConfiguration)
    {
        Configuration = theConfiguration;
    }
    
    internal class TelemetrySettings
    {
        internal Uri ConnectionString => new(Configuration["ConnectionStrings:Telemetry"]!);
    }
    
    internal class YarpSettings
    {
        internal IReadOnlyList<RouteConfig> Routes => Configuration.GetSection("ReverseProxy:Routes").Get<List<RouteConfig>>()!;
        internal IReadOnlyList<ClusterConfig> Clusters => Configuration.GetSection("ReverseProxy:Clusters").Get<List<ClusterConfig>>()!;
    }
}
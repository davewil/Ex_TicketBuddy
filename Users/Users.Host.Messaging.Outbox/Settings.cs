using Microsoft.Extensions.Configuration;

namespace Users.Host.Messaging.Outbox;

internal class Settings
{
    private static IConfiguration Configuration = null!;
    internal DatabaseSettings Database => new();
    internal RabbitMqSettings RabbitMq => new();
    internal TelemetrySettings Telemetry => new();
   
    internal Settings(IConfiguration theConfiguration)
    {
        Configuration = theConfiguration;
    }
    
    internal class RabbitMqSettings
    {
        internal Uri ConnectionString => new(Configuration["ConnectionStrings:Messaging"]!);
    }
   
    internal class TelemetrySettings
    {
        internal string ConnectionString => Configuration["ConnectionStrings:Telemetry"]!;
    }

    internal class DatabaseSettings
    {
        public string Connection => Configuration["ConnectionStrings:TicketBuddyUsers"]!;
    }
}
using Microsoft.Extensions.Configuration;

namespace Host.Messaging.Outbox;

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
        internal string Host => Configuration["RabbitMq:HostName"]!;
        internal string Username => Configuration["RabbitMq:Username"]!;
        internal string Password => Configuration["RabbitMq:Password"]!;
        internal string VirtualHost => Configuration["RabbitMq:VirtualHost"]!;
    }
   
    internal class TelemetrySettings
    {
        internal string ConnectionString => Configuration["Telemetry:ConnectionString"]!;
    }

    internal class DatabaseSettings
    {
        public string Connection => Configuration["ConnectionString"]!;
    }
}
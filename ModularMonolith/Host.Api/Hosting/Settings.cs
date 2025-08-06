namespace Api.Hosting;

internal class Settings
{
    private static IConfiguration Configuration = null!;
    internal DatabaseSettings Database => new();
    internal TelemetrySettings Telemetry => new();
   
    internal Settings(IConfiguration theConfiguration)
    {
        Configuration = theConfiguration;
    }
    
    internal class TelemetrySettings
    {
        internal string ConnectionString => Configuration["ConnectionStrings:Telemetry"]!;
    }

    internal class DatabaseSettings
    {
        public string Connection => Configuration["ConnectionStrings:TicketBuddy"]!;
    }
}
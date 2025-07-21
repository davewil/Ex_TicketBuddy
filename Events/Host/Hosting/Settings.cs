namespace Api.Hosting;

internal class Settings
{
    private static IConfiguration Configuration = null!;
    internal TelemetrySettings Telemetry => new();
    internal DatabaseSettings Database => new();
   
    internal Settings(IConfiguration theConfiguration)
    {
        Configuration = theConfiguration;
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
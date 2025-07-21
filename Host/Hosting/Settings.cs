namespace Api.Hosting;

internal class Settings
{
    private static IConfiguration Configuration = null!;
    internal ApplicationInsightsSettings ApplicationInsights => new();
    internal DatabaseSettings Database => new();
   
    internal Settings(IConfiguration theConfiguration)
    {
        Configuration = theConfiguration;
    }
   
    internal class ApplicationInsightsSettings
    {
        internal string ConnectionString => Configuration["ApplicationInsights:ConnectionString"]!;
    }

    internal class DatabaseSettings
    {
        public string Connection => Configuration["ConnectionString"]!;
    }
}
using Microsoft.Extensions.Configuration;

namespace TicketBuddy.DataSeeder;

internal class Settings
{
    private static IConfiguration Configuration = null!;
    internal ApiSettings Api => new();
   
    internal Settings(IConfiguration theConfiguration)
    {
        Configuration = theConfiguration;
    }
    
    internal class ApiSettings
    {
        public Uri BaseUrl => new (Configuration["ApiSettings:BaseUrl"]!);
    }
}
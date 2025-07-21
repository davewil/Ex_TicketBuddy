using Microsoft.Extensions.Configuration;

namespace Migrations.Host;

public static class Settings
{
    private static readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false).Build();

    public static class Database
    {
        public static string Connection => configuration["ConnectionString"]!;
    }
}
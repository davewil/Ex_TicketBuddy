using Microsoft.Extensions.Configuration;
using Shared.Hosting;

namespace Migrations.Host;

public static class Settings
{
    private static readonly IConfiguration configuration = Configuration.Build();

    public static class Database
    {
        public static string Connection => configuration["ConnectionStrings:TicketBuddyUsers"]!;
    }
}
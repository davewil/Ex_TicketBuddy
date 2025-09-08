using Infrastructure.Events.Configuration;
using Infrastructure.Tickets.Configuration;
using Infrastructure.Users.Configuration;

namespace Api.Hosting;

internal static class Database
{
    internal static void ConfigureDatabase(this IServiceCollection services, string connectionString)
    {
        services.ConfigureUsersDatabase(connectionString);
        services.ConfigureEventsDatabase(connectionString);
        services.ConfigureTicketsDatabase(connectionString);
    }
}
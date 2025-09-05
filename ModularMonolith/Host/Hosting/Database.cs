using Infrastructure.Events.Configuration;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Tickets.Configuration;
using Users.Persistence;

namespace Api.Hosting;

internal static class Database
{
    internal static void ConfigureDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<UserDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });
        services.ConfigureEventsDatabase(connectionString);
        services.ConfigureTicketsDatabase(connectionString);
    }
}
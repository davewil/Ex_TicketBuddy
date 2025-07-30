using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Host.Messaging.Outbox.Hosting;

internal static class Database
{
    internal static void ConfigureDatabase(this IServiceCollection services, Settings settings)
    {
        services.AddDbContext<EventDbContext>(options =>
        {
            options.UseSqlServer(settings.Database.Connection, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });
        services.AddDbContext<UserDbContext>(options =>
        {
            options.UseSqlServer(settings.Database.Connection, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });
    }
}
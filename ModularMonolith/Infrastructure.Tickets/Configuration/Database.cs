using Infrastructure.Tickets.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheDatabase = Infrastructure.Queries.Database;

namespace Infrastructure.Tickets.Configuration;

public static class Database
{
    public static void ConfigureTicketsDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TicketDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });
        services.AddScoped<TheDatabase>(_ => new TheDatabase(connectionString));
    }
}
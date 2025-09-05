using Infrastructure.Events.Configuration;
using Infrastructure.Tickets.Configuration;
using WebHost;
using UserRepository = Users.Persistence.UserRepository;

namespace Api.Hosting;

public static class Services
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.ConfigureEventsServices();
        services.ConfigureTicketsServices();
        services.AddCorsAllowAll();
    }
}
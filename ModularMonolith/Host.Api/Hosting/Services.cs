using Application;
using Application.Events;
using Events.Persistence;
using Users.Persistence;
using WebHost;

namespace Api.Hosting;

public static class Services
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<EventRepository>();
        services.AddScoped<EventService>();
        services.AddScoped<UserRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<Persistence.Tickets.Events.EventRepository>();
        services.AddCorsAllowAll();
    }
}
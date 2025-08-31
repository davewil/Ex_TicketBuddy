using Application;
using Application.Events;
using Application.Tickets;
using Persistence.Tickets;
using Users.Persistence;
using WebHost;
using EventRepository = Events.Persistence.EventRepository;

namespace Api.Hosting;

public static class Services
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<EventRepository>();
        services.AddScoped<EventService>();
        services.AddScoped<UserRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<Persistence.Tickets.EventRepository>();
        services.AddScoped<TicketRepository>();
        services.AddScoped<TicketService>();
        services.AddCorsAllowAll();
    }
}
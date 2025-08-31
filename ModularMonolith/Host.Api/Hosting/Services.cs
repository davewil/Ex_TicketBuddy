using Application;
using Application.Events;
using Application.Tickets;
using Persistence.Tickets;
using WebHost;
using EventRepository = Events.Persistence.EventRepository;
using UserRepository = Users.Persistence.UserRepository;

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
        services.AddScoped<Persistence.Tickets.UserRepository>();
        services.AddScoped<TicketRepository>();
        services.AddScoped<TicketService>();
        services.AddCorsAllowAll();
    }
}
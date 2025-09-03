using Persistence.Tickets;
using Persistence.Tickets.Commands;
using Persistence.Tickets.Queries;
using WebHost;
using EventRepository = Events.Persistence.EventRepository;
using UserRepository = Users.Persistence.UserRepository;

namespace Api.Hosting;

public static class Services
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<EventRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<Persistence.Tickets.Commands.EventRepository>();
        services.AddScoped<Persistence.Tickets.Commands.UserRepository>();
        services.AddScoped<TicketRepository>();
        services.AddScoped<ReadOnlyTicketRepository>();
        services.AddCorsAllowAll();
    }
}
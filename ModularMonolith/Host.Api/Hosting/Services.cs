using Infrastructure.Tickets;
using Infrastructure.Tickets.Commands;
using Infrastructure.Tickets.Queries;
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
        services.AddScoped<Infrastructure.Tickets.Commands.EventRepository>();
        services.AddScoped<Infrastructure.Tickets.Commands.UserRepository>();
        services.AddScoped<WriteOnlyTicketRepository>();
        services.AddScoped<ReadOnlyTicketRepository>();
        services.AddCorsAllowAll();
    }
}
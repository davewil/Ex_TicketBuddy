using Application.Tickets;
using Domain.Events.Messaging;
using Integration.Users.Messaging.Messages;
using MassTransit;
using Users.Persistence;

namespace Api.Hosting;

internal static class Messaging
{
    internal static void ConfigureMessaging(this IServiceCollection services, Settings settings)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            var eventsDomainAssembly = EventsDomainMessaging.Assembly;
            x.AddConsumers(eventsDomainAssembly);
            
            var ticketsMessagingAssembly = TicketsMessaging.Assembly;
            x.AddConsumers(ticketsMessagingAssembly);
            
            var usersDomainAssembly = UsersDomainMessaging.Assembly;
            x.AddConsumers(usersDomainAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.RabbitMq.ConnectionString);
                cfg.ReceiveEndpoint("tickets-inbox-events", e =>
                {
                    e.Bind<Integration.Events.Messaging.EventUpserted>();
                });
                cfg.ReceiveEndpoint("tickets-inbox-users", e =>
                {
                    e.Bind<UserUpserted>();
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
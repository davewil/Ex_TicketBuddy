using Application.Tickets;
using Domain.Events.Messaging;
using Integration.Tickets.Messaging.Inbound;
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
            
            var ticketsIntegrationInboundAssembly = TicketsIntegrationMessagingInbound.Assembly;
            x.AddConsumers(ticketsIntegrationInboundAssembly);
            
            var ticketsDomainMessagingAssembly = TicketsDomainMessaging.Assembly;
            x.AddConsumers(ticketsDomainMessagingAssembly);
            
            var usersDomainAssembly = UsersDomainMessaging.Assembly;
            x.AddConsumers(usersDomainAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.RabbitMq.ConnectionString);
                cfg.ReceiveEndpoint("tickets-inbox-events", e =>
                {
                    e.Bind<Integration.Events.Messaging.Outbound.EventUpserted>();
                });
                cfg.ReceiveEndpoint("tickets-inbox-users", e =>
                {
                    e.Bind<Integration.Users.Messaging.Outbound.Messages.UserUpserted>();
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
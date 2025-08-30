using Domain.Events.Messaging;
using Integration.Tickets.Messaging;
using MassTransit;

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

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.RabbitMq.ConnectionString);
                cfg.ReceiveEndpoint("tickets-inbox-events", e =>
                {
                    e.Bind<Integration.Events.Messaging.EventUpserted>();
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
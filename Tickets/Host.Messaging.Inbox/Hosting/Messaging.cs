using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Integration.Messaging.Inbound;

namespace Host.Messaging.Outbox.Hosting;

internal static class Messaging
{
    internal static void ConfigureMessaging(this IServiceCollection services, Settings settings)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.SetInMemorySagaRepositoryProvider();
            var ticketsIntegrationInboundAssembly = TicketsIntegrationMessagingInbound.Assembly;
            x.AddConsumers(ticketsIntegrationInboundAssembly);
            x.AddSagaStateMachines(ticketsIntegrationInboundAssembly);
            x.AddSagas(ticketsIntegrationInboundAssembly);
            x.AddActivities(ticketsIntegrationInboundAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {                        
                cfg.Host(settings.RabbitMq.Host, settings.RabbitMq.VirtualHost, h =>
                {
                    h.Username(settings.RabbitMq.Username);
                    h.Password(settings.RabbitMq.Password);
                });
                
                cfg.ReceiveEndpoint("tickets-inbox", e =>
                {
                    e.Bind<Events.Integration.Messaging.Outbound.Messages.EventUpserted>();
                    e.Bind<Events.Integration.Messaging.Outbound.Messages.EventDeleted>();
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
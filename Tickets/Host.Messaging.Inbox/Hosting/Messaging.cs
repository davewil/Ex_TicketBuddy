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
            var ticketsIntegrationInboundAssembly = TicketsIntegrationMessagingInbound.Assembly;
            x.AddConsumers(ticketsIntegrationInboundAssembly);

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
                    e.Bind<Users.Integration.Messaging.Outbound.Messages.UserUpserted>();
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
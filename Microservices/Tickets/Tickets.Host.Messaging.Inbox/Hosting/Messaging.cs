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
                cfg.Host(settings.RabbitMq.ConnectionString);
                
                cfg.ReceiveEndpoint("tickets-inbox-events", e =>
                {
                    e.Bind<Events.Integration.Messaging.Outbound.Messages.EventUpserted>();
                });

                cfg.ReceiveEndpoint("tickets-inbox-users", e =>
                {
                    e.Bind<Users.Integration.Messaging.Outbound.Messages.UserUpserted>();
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
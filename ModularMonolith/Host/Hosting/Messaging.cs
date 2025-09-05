using Application.Tickets;
using Integration.Users.Messaging.Messages;
using MassTransit;

namespace Api.Hosting;

internal static class Messaging
{
    internal static void ConfigureMessaging(this IServiceCollection services, Settings settings)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            
            var ticketsMessagingAssembly = TicketsMessaging.Assembly;
            x.AddConsumers(ticketsMessagingAssembly);
            
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
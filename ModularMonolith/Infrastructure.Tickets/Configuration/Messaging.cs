using Application.Tickets;
using Domain.Tickets;
using Integration.Users.Messaging.Messages;
using MassTransit;

namespace Infrastructure.Tickets.Configuration;

public static class Messaging
{
    public static void AddTicketsConsumers(this IBusRegistrationConfigurator x)
    {
        var ticketsIntegrationMessagingAssembly = TicketsIntegrationMessaging.Assembly;
        x.AddConsumers(ticketsIntegrationMessagingAssembly);
        
        var ticketsDomainMessagingAssembly = TicketsDomainMessaging.Assembly;
        x.AddConsumers(ticketsDomainMessagingAssembly);
    }
    
    public static void ConfigureTicketsMessaging(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.ReceiveEndpoint("tickets-inbox-events", e =>
        {
            e.Bind<Integration.Events.Messaging.EventUpserted>();
        });
        cfg.ReceiveEndpoint("tickets-inbox-users", e =>
        {
            e.Bind<UserUpserted>();
        });
    }
}
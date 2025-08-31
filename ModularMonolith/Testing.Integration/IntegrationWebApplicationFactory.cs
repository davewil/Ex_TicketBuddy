using Api.Hosting;
using Domain.Events.Messaging;
using Domain.Users.Messaging;
using Integration.Events.Messaging.Inbound;
using Integration.Tickets.Messaging.Inbound;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Integration;

public class IntegrationWebApplicationFactory<TProgram>(string connectionString)
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.ConfigureDatabase(connectionString);
            services.ConfigureServices();
            services.AddMassTransitTestHarness(x =>
            {
                var applicationAssembly = EventsDomainMessaging.Assembly;
                x.AddConsumers(applicationAssembly);
                
                var eventsIntegrationInboundAssembly = EventsIntegrationMessagingInbound.Assembly;
                x.AddConsumers(eventsIntegrationInboundAssembly);
                
                var ticketsIntegrationInboundAssembly = TicketsIntegrationMessagingInbound.Assembly;
                x.AddConsumers(ticketsIntegrationInboundAssembly);

                var usersApplicationAssembly = UsersDomainMessaging.Assembly;
                x.AddConsumers(usersApplicationAssembly);
            });
        });

        builder.UseEnvironment("Test");
    }
}
using Api.Hosting;
using Application.Tickets;
using Infrastructure.Tickets.Configuration;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Integration;

public class IntegrationWebApplicationFactory<TProgram>(string connectionString, string? redisConnectionString = null)
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            if (redisConnectionString is not null) services.ConfigureCache(redisConnectionString);
            services.ConfigureDatabase(connectionString);
            services.ConfigureServices();
            services.AddMassTransitTestHarness(x =>
            {
                var ticketsDomainMessagingAssembly = TicketsMessaging.Assembly;
                x.AddConsumers(ticketsDomainMessagingAssembly);
            });
        });

        builder.UseEnvironment("Test");
    }
}
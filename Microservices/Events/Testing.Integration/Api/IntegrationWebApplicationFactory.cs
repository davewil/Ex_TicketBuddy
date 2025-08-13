using Events.Application;
using Events.Domain.Messaging;
using Events.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Api;

public class IntegrationWebApplicationFactory<TProgram>(string connectionString)
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddDbContext<EventDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });
            services.AddScoped<EventRepository>();
            services.AddScoped<EventService>();
            services.AddMassTransitTestHarness(x =>
            {
                var applicationAssembly = EventsDomainMessaging.Assembly;
                x.AddConsumers(applicationAssembly);
            });
        });

        builder.UseEnvironment("Test");
    }
}
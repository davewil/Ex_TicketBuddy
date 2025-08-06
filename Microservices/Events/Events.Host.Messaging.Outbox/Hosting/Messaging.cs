using Events.Domain.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Events.Host.Messaging.Outbox.Hosting;

internal static class Messaging
{
    internal static void ConfigureMessaging(this IServiceCollection services, Settings settings)
    {
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<EventDbContext>(o =>
            {
                o.UseSqlServer();
                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });
            x.AddConfigureEndpointsCallback((context, _, cfg) =>
            {
                cfg.UseEntityFrameworkOutbox<EventDbContext>(context);
            });

            x.SetKebabCaseEndpointNameFormatter();
            var eventsDomainAssembly = EventsDomainMessaging.Assembly;
            x.AddConsumers(eventsDomainAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.RabbitMq.ConnectionString);
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
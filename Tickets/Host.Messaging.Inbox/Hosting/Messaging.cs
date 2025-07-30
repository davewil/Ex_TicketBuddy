using Events.Integration.Messaging;
using Integration.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Host.Messaging.Outbox.Hosting;

internal static class Messaging
{
    internal static void ConfigureMessaging(this IServiceCollection services, Settings settings)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.SetInMemorySagaRepositoryProvider();
            var applicationAssembly = EventsMessaging.Assembly;
            x.AddConsumers(applicationAssembly);
            x.AddSagaStateMachines(applicationAssembly);
            x.AddSagas(applicationAssembly);
            x.AddActivities(applicationAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {                        
                cfg.Host(settings.RabbitMq.Host, settings.RabbitMq.VirtualHost, h =>
                {
                    h.Username(settings.RabbitMq.Username);
                    h.Password(settings.RabbitMq.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
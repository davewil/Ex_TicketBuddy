using MassTransit;
using Messaging;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Host.Messaging.Outbox.Hosting;

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
            x.SetInMemorySagaRepositoryProvider();
            var applicationAssembly = MessagingAssembly.Assembly;
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
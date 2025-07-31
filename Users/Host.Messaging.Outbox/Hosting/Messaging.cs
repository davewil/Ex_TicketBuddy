using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Users.Domain.Messaging;

namespace Host.Messaging.Outbox.Hosting;

internal static class Messaging
{
    internal static void ConfigureMessaging(this IServiceCollection services, Settings settings)
    {
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<UserDbContext>(o =>
            {
                o.UseSqlServer();
                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });
            x.AddConfigureEndpointsCallback((context, _, cfg) =>
            {
                cfg.UseEntityFrameworkOutbox<UserDbContext>(context);
            });

            x.SetKebabCaseEndpointNameFormatter();
            var eventsDomainAssembly = UsersDomainMessaging.Assembly;
            x.AddConsumers(eventsDomainAssembly);

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
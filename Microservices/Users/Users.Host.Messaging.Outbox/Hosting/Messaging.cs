using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Users.Persistence;
using Users.Domain.Messaging;

namespace Users.Host.Messaging.Outbox.Hosting;

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
            var usersDomainAssembly = UsersDomainMessaging.Assembly;
            x.AddConsumers(usersDomainAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.RabbitMq.ConnectionString);

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
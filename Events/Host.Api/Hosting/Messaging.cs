using MassTransit;
using Persistence;

namespace Api.Hosting;

internal static class Messaging
{
    internal static void ConfigureMessaging(this IServiceCollection services, Settings settings)
    {
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<EventDbContext>(o =>
            {
                o.UseSqlServer();
                o.UseBusOutbox();
            });
            x.UsingRabbitMq((context, cfg) =>
            {                        
                cfg.Host(settings.RabbitMq.Host, settings.RabbitMq.VirtualHost, h =>
                {
                    h.Username(settings.RabbitMq.Username);
                    h.Password(settings.RabbitMq.Password);
                });
            });
        });
    }
}
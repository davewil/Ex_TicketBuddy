using Host.Messaging.Outbox;
using Host.Messaging.Outbox.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Persistence.Events;
using Persistence.Users;

var settings = new Settings(Configuration.Build());
var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.ConfigureDatabase(settings);
        services.ConfigureTelemetry(settings, "TicketApi.Inbox");
        services.ConfigureMessaging(settings);
        services.AddScoped<EventRepository>();
        services.AddScoped<UserRepository>();
    })
    .Build();

await host.RunAsync();
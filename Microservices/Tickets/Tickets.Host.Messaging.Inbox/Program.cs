using Host.Messaging.Outbox;
using Host.Messaging.Outbox.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tickets.Persistence;
using Tickets.Persistence.Events;
using Tickets.Persistence.Users;
using Shared.Hosting;

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
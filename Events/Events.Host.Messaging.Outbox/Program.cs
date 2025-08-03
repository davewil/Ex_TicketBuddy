using Events.Host.Messaging.Outbox;
using Events.Host.Messaging.Outbox.Hosting;
using Host.Messaging.Outbox.Hosting;
using Microsoft.Extensions.Hosting;
using Shared.Hosting;

var settings = new Settings(Configuration.Build());
var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.ConfigureDatabase(settings);
        services.ConfigureTelemetry(settings, "EventApi.Outbox");
        services.ConfigureMessaging(settings);
    })
    .Build();

await host.RunAsync();
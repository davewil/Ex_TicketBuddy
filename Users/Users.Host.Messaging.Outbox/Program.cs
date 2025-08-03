using Microsoft.Extensions.Hosting;
using Shared.Hosting;
using Users.Host.Messaging.Outbox;
using Users.Host.Messaging.Outbox.Hosting;

var settings = new Settings(Configuration.Build());
var host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.ConfigureDatabase(settings);
        services.ConfigureTelemetry(settings, "UserApi.Outbox");
        services.ConfigureMessaging(settings);
    })
    .Build();

await host.RunAsync();
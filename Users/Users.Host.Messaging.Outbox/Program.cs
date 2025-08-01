using Host.Messaging.Outbox;
using Host.Messaging.Outbox.Hosting;
using Microsoft.Extensions.Hosting;

var settings = new Settings(Configuration.Build());
var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.ConfigureDatabase(settings);
        services.ConfigureTelemetry(settings, "UserApi.Outbox");
        services.ConfigureMessaging(settings);
    })
    .Build();

await host.RunAsync();
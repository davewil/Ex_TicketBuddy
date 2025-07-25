using Microsoft.Extensions.Configuration;

namespace Host.Messaging.Outbox.Hosting;

internal static class Configuration
{
    internal static IConfigurationRoot Build()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables();
        return configurationBuilder.Build();
    }
}
using Common.Environment;
using Microsoft.Extensions.Configuration;

namespace Shared.Hosting;

public static class Configuration
{
    public static IConfigurationRoot Build()
    {
        var environment = CommonEnvironmentExtensions.GetEnvironment();
        
        if (environment is CommonEnvironment.LocalDevelopment)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.local.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();
            return configurationBuilder.Build();  
        }
        
        var theConfigurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.docker.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables();
        return theConfigurationBuilder.Build();
    }
}
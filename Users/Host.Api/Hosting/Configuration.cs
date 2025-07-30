using Common.Environment;

namespace Api.Hosting;

internal static class Configuration
{
    internal static IConfigurationRoot Build()
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
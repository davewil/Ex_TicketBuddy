namespace Api.Hosting;

internal static class Configuration
{
    internal static IConfigurationRoot Build()
    {
        var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        return configurationBuilder.Build();
    }
}
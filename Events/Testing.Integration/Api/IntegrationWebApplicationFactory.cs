using Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Repositories;

namespace Integration.Api;

public class IntegrationWebApplicationFactory<TProgram>(string connectionString)
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<Db>(_ => new Db(connectionString));
            services.AddScoped<EventRepository>();
            services.AddScoped<EventService>();
        });

        builder.UseEnvironment("Test");
    }
}
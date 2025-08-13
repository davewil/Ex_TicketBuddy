using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Users.Application;
using Users.Domain.Messaging;
using Users.Persistence;

namespace Integration.Api;

public class IntegrationWebApplicationFactory<TProgram>(string connectionString)
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });
            services.AddScoped<UserRepository>();
            services.AddScoped<UserService>();
            services.AddMassTransitTestHarness(x =>
            {
                var applicationAssembly = UsersDomainMessaging.Assembly;
                x.AddConsumers(applicationAssembly);
            });
        });

        builder.UseEnvironment("Test");
    }
}
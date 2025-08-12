using System.Net.Http.Json;
using Api.Hosting;
using Api.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Hosting;

namespace TicketBuddy.DataSeeder;

public static class Program
{
    public static async Task Main()
    {
        var configuration = Configuration.Build();
        var settings = new Settings(configuration);

        var serviceProvider = new ServiceCollection()
            .AddLogging(builder => builder.AddConsole())
            .AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = settings.Api.BaseUrl;
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            })
            .Services
            .BuildServiceProvider();

        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient("ApiClient");
        
        await CreateAdministratorUser(httpClient);
        await CreateCustomerUsers(httpClient);
        await CreateFutureEvents(httpClient);
    }

    private static async Task CreateAdministratorUser(HttpClient client)
    {
        var payload = new UserPayload(
            "Admin User",
            "admin@ticketbuddy.com"
        );

        var response = await client.PostAsJsonAsync(UserRoutes.Users, payload);
        response.EnsureSuccessStatusCode();
    }

    private static async Task CreateCustomerUsers(HttpClient client)
    {
        var customerData = new[]
        {
            (Name: "John Smith", Email: "john.smith@example.com"),
            (Name: "Jane Doe", Email: "jane.doe@example.com"),
            (Name: "Robert Johnson", Email: "robert.johnson@example.com"),
            (Name: "Emily Davis", Email: "emily.davis@example.com")
        };

        foreach (var customer in customerData)
        {
            var payload = new UserPayload(
                customer.Name,
                customer.Email
            );

            var response = await client.PostAsJsonAsync(UserRoutes.Users, payload);
            response.EnsureSuccessStatusCode();
        }
    }

    private static async Task CreateFutureEvents(HttpClient client)
    {
        var eventData = new[]
        {
            "Summer Rock Festival",
            "Classical Symphony",
            "International Football Match",
            "Comedy Night Special",
            "Tech Conference"
        };

        foreach (var eventInfo in eventData)
        {
            var payload = new EventPayload(
                eventInfo
            );

            var response = await client.PostAsJsonAsync(EventsRoutes.Events, payload);
            response.EnsureSuccessStatusCode();
        }
    }
}


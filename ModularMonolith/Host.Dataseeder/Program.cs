using System.Net.Http.Json;
using Controllers.Events.Requests;
using Controllers.Users.Requests;
using Dataseeder.Hosting;
using Domain.Events.Primitives;
using Domain.Users.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserRoutes = Controllers.Users.Routes;
using EventRoutes = Controllers.Events.Routes;

namespace Dataseeder;

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
            "admin@ticketbuddy.com",
            UserType.Administrator
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
                customer.Email,
                UserType.Customer
            );

            var response = await client.PostAsJsonAsync(UserRoutes.Users, payload);
            response.EnsureSuccessStatusCode();
        }
    }

    private static async Task CreateFutureEvents(HttpClient client)
    {
        var eventData = new[]
        {
            (Name: "Summer Rock Festival", StartDate: DateTimeOffset.Now.AddDays(30), EndDate: DateTimeOffset.Now.AddDays(30).AddHours(1), Venue: Venue.O2ArenaLondon),
            (Name: "Classical Symphony", StartDate: DateTimeOffset.Now.AddDays(45), EndDate: DateTimeOffset.Now.AddDays(45).AddHours(1),Venue: Venue.RoyalAlbertHallLondon),
            (Name: "International Football Match", StartDate: DateTimeOffset.Now.AddDays(60), EndDate: DateTimeOffset.Now.AddDays(60).AddHours(1),Venue: Venue.WembleyStadiumLondon),
            (Name: "Comedy Night Special", StartDate: DateTimeOffset.Now.AddDays(15), EndDate: DateTimeOffset.Now.AddDays(15).AddHours(1),Venue: Venue.ManchesterArena),
            (Name: "Tech Conference 2025", StartDate: DateTimeOffset.Now.AddDays(90), EndDate: DateTimeOffset.Now.AddDays(90).AddHours(1), Venue: Venue.PrincipalityStadiumCardiff)
        };

        foreach (var eventInfo in eventData)
        {
            var payload = new EventPayload(
                eventInfo.Name,
                eventInfo.StartDate,
                eventInfo.EndDate,
                eventInfo.Venue
            );

            var response = await client.PostAsJsonAsync(EventRoutes.Events, payload);
            response.EnsureSuccessStatusCode();
        }
    }
}


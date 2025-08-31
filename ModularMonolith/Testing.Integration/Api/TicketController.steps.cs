using System.Net;
using System.Text;
using BDD;
using Controllers.Tickets;
using Controllers.Tickets.Requests;
using Domain.Events.Primitives;
using Integration.Events.Messaging;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Migrations;
using Shouldly;
using Testcontainers.MsSql;

namespace Integration.Api;

public partial class TicketControllerSpecs : TruncateDbSpecification
{
    private IntegrationWebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;
    private HttpContent content = null!;

    private readonly Guid event_id = Guid.NewGuid();
    private readonly decimal price = 25.00m;
    private HttpStatusCode response_code;
    private const string application_json = "application/json";
    private const string name = "wibble";
    private readonly DateTimeOffset event_start_date = DateTimeOffset.Now.AddDays(1);
    private readonly DateTimeOffset event_end_date = DateTimeOffset.Now.AddDays(1).AddHours(2);
    private static MsSqlContainer database = null!;
    private ITestHarness testHarness = null!;

    protected override void before_all()
    {
        database = new MsSqlBuilder().WithPortBinding(1433, true).Build();
        database.StartAsync().Await();
        database.ExecScriptAsync("CREATE DATABASE [TicketBuddy.Tickets]").GetAwaiter().GetResult();
        Migration.Upgrade(database.GetTicketBuddyConnectionString());
    }
    
    protected override void before_each()
    {
        base.before_each();
        content = null!;
        factory = new IntegrationWebApplicationFactory<Program>(database.GetTicketBuddyConnectionString());
        client = factory.CreateClient();
        testHarness = factory.Services.GetRequiredService<ITestHarness>();
        testHarness.Start().Await();
    }

    protected override void after_each()
    {
        Truncate(database.GetTicketBuddyConnectionString());
        testHarness.Stop().Await();
        client.Dispose();
        factory.Dispose();
    }

    protected override void after_all()
    {
        database.StopAsync().Await();
        database.DisposeAsync().GetAwaiter().GetResult();
    }

    private void an_event_exists()
    {
        testHarness.Bus.Publish(new EventUpserted
        {
            Id = event_id,
            EventName = name,
            StartDate = event_start_date,
            EndDate = event_end_date,
            Venue = Venue.EmiratesOldTraffordManchester
        });
        testHarness.Consumed.Any<EventUpserted>(x => x.Context.Message.Id == event_id).Await();
    }
    
    private void create_content()
    {
        content = new StringContent(
            JsonSerialization.Serialize(new TicketPayload(price)),
            Encoding.UTF8,
            application_json);
    }

    private void releasing_the_tickets()
    {
        create_content();
        var response = client.PostAsync(Routes.Events + $"/{event_id}" + "/tickets", content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
        response_code.ShouldBe(HttpStatusCode.Created);
    }

    private void requesting_the_tickets()
    {
        var response = client.GetAsync(Routes.Events + $"/{event_id}" + "/tickets").GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }

    private void the_tickets_are_released()
    {
        response_code.ShouldBe(HttpStatusCode.OK);
        var tickets = JsonSerialization.Deserialize<IList<Domain.Tickets.Entities.Ticket>>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        tickets.Count.ShouldBe(17);
        tickets = tickets.OrderBy(t => t.SeatNumber).ToList();
        uint counter = 1;
        foreach (var ticket in tickets)
        {
            ticket.EventId.ShouldBe(event_id);
            ticket.Price.ShouldBe(price);
            ticket.SeatNumber.ShouldBe(counter);
            counter++;
        }
    }
}
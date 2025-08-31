using System.Net;
using System.Text;
using BDD;
using Controllers.Tickets;
using Controllers.Tickets.Requests;
using Domain.Events.Primitives;
using Integration.Events.Messaging;
using Integration.Users.Messaging.Messages;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Migrations;
using Shouldly;
using Testcontainers.MsSql;
using WebHost;

namespace Integration.Api;

public partial class TicketControllerSpecs : TruncateDbSpecification
{
    private IntegrationWebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;
    private HttpContent content = null!;

    private readonly Guid event_id = Guid.NewGuid();
    private readonly Guid user_id = Guid.NewGuid();
    private const decimal price = 25.00m;
    private HttpStatusCode response_code;
    private const string application_json = "application/json";
    private const string name = "wibble";
    private const string full_name = "John Smith";
    private const string email = "john.smith@gmail.com";
    private readonly DateTimeOffset event_start_date = DateTimeOffset.Now.AddDays(1);
    private readonly DateTimeOffset event_end_date = DateTimeOffset.Now.AddDays(1).AddHours(2);
    private static MsSqlContainer database = null!;
    private ITestHarness testHarness = null!;
    private Guid[] ticket_ids = null!;

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
        ticket_ids = [];
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

    private void a_user_exists()
    {
        testHarness.Bus.Publish(new UserUpserted
        {
            Id = user_id,
            FullName = full_name,
            Email = email
        });
        testHarness.Consumed.Any<UserUpserted>(x => x.Context.Message.Id == user_id).Await();
    }

    private void tickets_are_released()
    {
        releasing_the_tickets();
        var ticketsRequest = client.GetAsync(Routes.EventTickets(event_id)).GetAwaiter().GetResult();
        ticketsRequest.StatusCode.ShouldBe(HttpStatusCode.OK);
        var tickets = JsonSerialization.Deserialize<IList<Domain.Tickets.Entities.Ticket>>(ticketsRequest.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        ticket_ids = tickets.Select(t => t.Id).ToArray();
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
        var response = client.PostAsync(Routes.EventTickets(event_id), content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }

    private void requesting_the_tickets()
    {
        response_code.ShouldBe(HttpStatusCode.Created);
        var response = client.GetAsync(Routes.EventTickets(event_id)).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }

    private void purchasing_two_tickets()
    {
        content = new StringContent(
            JsonSerialization.Serialize(new TicketPurchasePayload(user_id, ticket_ids.Take(2).ToArray())),
            Encoding.UTF8,
            application_json);
        var response = client.PostAsync(Routes.EventTickets(event_id) + "/purchase", content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }

    private void two_tickets_are_purchased()
    {
        purchasing_two_tickets();
    }
    
    private void purchasing_two_tickets_again()
    {
        purchasing_two_tickets();
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

    private void the_tickets_are_purchased()
    {
        response_code.ShouldBe(HttpStatusCode.OK);
        var response = client.GetAsync(Routes.EventTickets(event_id)).GetAwaiter().GetResult();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var tickets = JsonSerialization.Deserialize<IList<Domain.Tickets.Entities.Ticket>>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        tickets.Count.ShouldBe(15);
        tickets.Where(t => ticket_ids.Take(2).Contains(t.Id)).ToList().Count.ShouldBe(0);
    }

    private void user_is_informed_that_tickets_have_already_been_released()
    {
        response_code.ShouldBe(HttpStatusCode.BadRequest);
        var theError = JsonSerialization.Deserialize<ApiError>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        theError.Errors.ShouldContain("Tickets have already been released for this event");
    }

    private void user_informed_they_cannot_purchase_tickets_that_are_purchased()
    {
        response_code.ShouldBe(HttpStatusCode.BadRequest);
        var theError = JsonSerialization.Deserialize<ApiError>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        theError.Errors.ShouldContain("Tickets are not available");
    }
}
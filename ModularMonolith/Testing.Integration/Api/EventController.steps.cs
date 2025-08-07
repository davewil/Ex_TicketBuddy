using System.Net;
using System.Text;
using BDD;
using Controllers.Events;
using Controllers.Events.Requests;
using Domain.Events.Entities;
using Migrations;
using Shouldly;
using Testcontainers.MsSql;
using WebHost;

namespace Integration.Api;

public partial class EventControllerSpecs : TruncateDbSpecification
{
    private IntegrationWebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;
    private HttpContent content = null!;

    private Guid returned_id;
    private Guid another_id;
    private HttpStatusCode response_code;
    private const string application_json = "application/json";
    private const string name = "wibble";
    private const string new_name = "wobble";
    private readonly DateTimeOffset event_date = DateTimeOffset.Now.AddDays(1);
    private readonly DateTimeOffset new_event_date = DateTimeOffset.Now.AddDays(2);
    private readonly DateTimeOffset past_event_date = DateTimeOffset.Now.AddDays(-1);
    private static MsSqlContainer database = null!;

    protected override void before_all()
    {
        database = new MsSqlBuilder().WithPortBinding(1433, true).Build();
        database.StartAsync().Await();
        database.ExecScriptAsync("CREATE DATABASE [TicketBuddy.Events]").GetAwaiter().GetResult();
        Migration.Upgrade(database.GetTicketBuddyConnectionString());
    }
    
    protected override void before_each()
    {
        base.before_each();
        content = null!;
        returned_id = Guid.Empty;
        factory = new IntegrationWebApplicationFactory<Program>(database.GetTicketBuddyConnectionString());
        client = factory.CreateClient();
    }

    protected override void after_each()
    {
        Truncate(database.GetTicketBuddyConnectionString());
        client.Dispose();
        factory.Dispose();
    }

    protected override void after_all()
    {
        database.StopAsync().Await();
        database.DisposeAsync().GetAwaiter().GetResult();
    }

    private void a_request_to_create_an_event()
    {
        create_content(name, event_date);
    }

    private void a_request_to_create_an_event_imminently()
    {
        create_content(name, DateTimeOffset.Now.AddSeconds(1));
    }
    
    private void a_request_to_create_an_event_with_a_date_in_the_past()
    {
        create_content(name, past_event_date);
    }
    
    private void a_request_to_update_the_event_with_a_date_in_the_past()
    {
        create_content(new_name, past_event_date);
    }

    private void create_content(string the_name, DateTimeOffset the_event_date)
    {
        content = new StringContent(
            JsonSerialization.Serialize(new EventPayload(the_name, the_event_date)),
            Encoding.UTF8,
            application_json);
    }

    private void a_request_to_create_another_event()
    {
        create_content(new_name, event_date);

    }
    
    private void a_request_to_update_the_event()
    {
        create_content(new_name, new_event_date);
    }

    private void creating_the_event()
    {
        var response = client.PostAsync(Routes.Events, content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
        response_code.ShouldBe(HttpStatusCode.Created);
        returned_id = JsonSerialization.Deserialize<Guid>(content.ReadAsStringAsync().Await());
    }    
    
    private void creating_the_event_that_will_fail()
    {
        var response = client.PostAsync(Routes.Events, content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }
    
    private void creating_another_event()
    {
        var response = client.PostAsync(Routes.Events, content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        another_id = JsonSerialization.Deserialize<Guid>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    }      
    
    private void updating_the_event()
    {
        var response = client.PutAsync(Routes.Events + $"/{returned_id}", content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        response_code.ShouldBe(HttpStatusCode.NoContent);
    }
    
    private void updating_the_event_that_will_fail()
    {
        var response = client.PutAsync(Routes.Events + $"/{returned_id}", content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }
    
    private void an_event_exists()
    {
        a_request_to_create_an_event();
        creating_the_event();
        returned_id = JsonSerialization.Deserialize<Guid>(content.ReadAsStringAsync().GetAwaiter().GetResult());
    }

    private static void a_short_wait()
    {
        Thread.Sleep(2000);
    }
    
    private void an_imminent_event_exists()
    {
        a_request_to_create_an_event_imminently();
        creating_the_event_that_will_fail();
    }
    
    private void another_event_exists()
    {
        a_request_to_create_another_event();
        creating_another_event();
    }

    private void requesting_the_event()
    {
        var response = client.GetAsync(Routes.Events + $"/{returned_id}").GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }
    
    private void requesting_the_updated_event()
    {
        var response = client.GetAsync(Routes.Events + $"/{returned_id}").GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }
    
    private void listing_the_events()
    {
        var response = client.GetAsync(Routes.Events).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }

    private void the_event_is_created()
    {

        var theEvent = JsonSerialization.Deserialize<Event>(content.ReadAsStringAsync().Await());
        response_code.ShouldBe(HttpStatusCode.OK);
        theEvent.Id.ShouldBe(returned_id);
        theEvent.EventName.ToString().ShouldBe(name);
        theEvent.Date.ShouldBe(event_date);
    }
    
    private void the_event_is_not_created()
    {
        response_code.ShouldBe(HttpStatusCode.BadRequest);
        var apiError = JsonSerialization.Deserialize<ApiError>(content.ReadAsStringAsync().Await());
        apiError.Errors[0].ShouldContain("Event date cannot be in the past");
    }
    
    private void the_event_is_not_updated()
    {
        response_code.ShouldBe(HttpStatusCode.BadRequest);
        var apiError = JsonSerialization.Deserialize<ApiError>(content.ReadAsStringAsync().Await());
        apiError.Errors[0].ShouldContain("Event date cannot be in the past");
    }
    
    private void the_event_is_updated()
    {
        var theEvent = JsonSerialization.Deserialize<Event>(content.ReadAsStringAsync().Await());
        response_code.ShouldBe(HttpStatusCode.OK);
        theEvent.Id.ShouldBe(returned_id);
        theEvent.EventName.ToString().ShouldBe(new_name);
        theEvent.Date.ShouldBe(new_event_date);
    }    
    
    private void the_events_are_listed()
    {
        var theEvent = JsonSerialization.Deserialize<IReadOnlyList<Event>>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        response_code.ShouldBe(HttpStatusCode.OK);
        theEvent.Count.ShouldBe(2);
        theEvent.Single(e => e.Id == returned_id).EventName.ToString().ShouldBe(name);
        theEvent.Single(e => e.Id == another_id).EventName.ToString().ShouldBe(new_name);
    }

    private void the_events_are_listed_without_the_past_event()
    {
        var theEvent = JsonSerialization.Deserialize<IReadOnlyList<Event>>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        response_code.ShouldBe(HttpStatusCode.OK);
        theEvent.Count.ShouldBe(1);
        theEvent.Single().Id.ShouldBe(returned_id);
        theEvent.Single().EventName.ToString().ShouldBe(name);
    }
}
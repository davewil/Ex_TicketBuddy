using NUnit.Framework;

namespace Integration.Api;

public partial class EventControllerSpecs
{
    [Test]
    public void can_create_event()
    {
        Given(a_request_to_create_an_event);
        When(creating_the_event);
        And(requesting_the_event);
        Then(the_event_is_created);
    }
    
    [Test]
    public void cannot_create_event_with_date_in_the_past()
    {
        Given(a_request_to_create_an_event_with_a_date_in_the_past);
        When(creating_the_event_that_will_fail);
        Then(the_event_is_not_created);
    }
    
    [Test]
    public void can_update_event()
    {
        Given(an_event_exists);
        And(a_request_to_update_the_event);
        When(updating_the_event);
        And(requesting_the_updated_event);
        Then(the_event_is_updated);
    }
    
    [Test]
    public void cannot_update_event_with_date_in_the_past()
    {
        Given(an_event_exists);
        And(a_request_to_update_the_event_with_a_date_in_the_past);
        When(updating_the_event_that_will_fail);
        Then(the_event_is_not_updated);
    }
    
    [Test]
    public void can_list_events()
    {
        Given(an_event_exists);
        And(another_event_exists);
        When(listing_the_events);
        Then(the_events_are_listed);
    }
}
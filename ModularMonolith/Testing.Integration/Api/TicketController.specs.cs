using NUnit.Framework;

namespace Integration.Api;

public partial class TicketControllerSpecs
{
    [Test]
    public void can_release_tickets()
    {
        Given(an_event_exists);
        When(releasing_the_tickets);
        And(requesting_the_tickets);
        Then(the_tickets_are_released);
        And(a_tickets_released_message_is_published);
    }

    [Test]
    public void cannot_release_tickets_for_an_event_twice()
    {
        Given(an_event_exists);
        And(tickets_are_released);
        When(releasing_the_tickets);
        Then(user_is_informed_that_tickets_have_already_been_released);
    }
    
    [Test]
    public void can_update_ticket_price_for_unpurchased_tickets()
    {
        Given(an_event_exists);
        And(tickets_are_released);
        And(a_user_exists);
        And(two_tickets_are_purchased);
        When(updating_the_ticket_prices);
        And(requesting_the_tickets);
        Then(the_ticket_prices_are_updated);
        And(purchased_tickets_are_not_updated);
    }

    [Test]
    public void user_can_purchase_two_tickets()
    {
        Given(an_event_exists);
        And(tickets_are_released);
        And(a_user_exists);
        When(purchasing_two_tickets);
        Then(the_tickets_are_purchased);
    }
    
    [Test]
    public void user_cannot_purchase_tickets_that_are_purchased()
    {
        Given(an_event_exists);
        And(tickets_are_released);
        And(a_user_exists);
        And(two_tickets_are_purchased);
        When(purchasing_two_tickets_again);
        Then(user_informed_they_cannot_purchase_tickets_that_are_purchased);
    }
}
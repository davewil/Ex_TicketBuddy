using NUnit.Framework;

namespace Integration.Api;

public partial class TicketControllerSpecs
{
    [Test]
    public void can_release_tickets()
    {
        Given(an_event_exists);
        When(requesting_the_tickets);
        Then(the_tickets_are_released);
    }
    
    [Test]
    public void can_update_ticket_price_for_unpurchased_tickets()
    {
        Given(an_event_exists);
        And(a_user_exists);
        And(requesting_the_tickets);
        And(two_tickets_are_purchased);
        When(updating_the_ticket_prices);
        Then(the_ticket_prices_are_updated);
        And(purchased_tickets_are_not_updated);
    }

    [Test]
    public void user_can_purchase_two_tickets()
    {
        Given(an_event_exists);
        And(a_user_exists);
        And(requesting_the_tickets);
        When(purchasing_two_tickets);
        Then(the_tickets_are_purchased);
    }
    
    [Test]
    public void user_cannot_purchase_tickets_that_are_purchased()
    {
        Given(an_event_exists);
        And(a_user_exists);
        And(requesting_the_tickets);
        And(two_tickets_are_purchased);
        When(purchasing_two_tickets_again);
        Then(user_informed_they_cannot_purchase_tickets_that_are_purchased);
    }
}
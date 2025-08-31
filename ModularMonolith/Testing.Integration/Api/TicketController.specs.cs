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
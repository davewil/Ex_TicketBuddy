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
}
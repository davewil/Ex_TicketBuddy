using BDD;
using Domain.Events.Entities;
using Domain.Events.Primitives;
using FluentAssertions;

namespace Unit.Domain;

public partial class EventSpecs : Specification
{
    private Guid id;
    private string name = null!;
    private DateTimeOffset date = DateTimeOffset.Now.AddDays(1);
    private Event user = null!;

    private const string invalid_name = "Jackie Chan 123!";
    private const string valid_name = "Jackie Chan 123";

    protected override void before_each()
    {
        base.before_each();
        id = Guid.NewGuid();
        name = null!;
        user = null!;
        date = DateTimeOffset.Now.AddDays(1);
    }

    private void valid_inputs()
    {
        name = valid_name;
        date = DateTimeOffset.Now.AddDays(1);
    }

    private void a_null_user_name()
    {
        name = null!;
    }    
    
    private void an_event_name()
    {
        name = string.Empty;
    }
    
    private void an_event_with_non_alphanumerical_characters()
    {
        name = invalid_name;
    }    
    
    private void creating_an_event()
    {
        user = new Event(id, name, date, Venue.FirstDirectArenaLeeds);
    }    
    
    private void the_event_is_created()
    {
        user.Id.Should().Be(id);
        user.EventName.ToString().Should().Be(valid_name);
    }
}
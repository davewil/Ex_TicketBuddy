using BDD;
using Domain.Events.Entitites;
using FluentAssertions;

namespace Unit.Domain;

public partial class EventSpecs : Specification
{
    private Guid id;
    private string name = null!;
    private Event user = null!;

    private const string invalid_name = "Jackie Chan 123";
    private const string valid_name = "Jackie Chan";

    protected override void before_each()
    {
        base.before_each();
        id = Guid.NewGuid();
        name = null!;
        user = null!;
    }

    private void valid_inputs()
    {
        name = valid_name;
    }

    private void a_null_user_name()
    {
        name = null!;
    }    
    
    private void an_event_name()
    {
        name = string.Empty;
    }     
    
    private void an_event_with_non_alphabetical_characters()
    {
        name = invalid_name;
    }    
    
    private void creating_an_event()
    {
        user = new Event(id, name);
    }    
    
    private void the_event_is_created()
    {
        user.Id.Should().Be(id);
        user.EventName.ToString().Should().Be(valid_name);
    }
}
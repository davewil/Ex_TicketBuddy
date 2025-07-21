using Domain.Primitives;

namespace Domain.Entities;

public class Event(Guid id, Name name) : Aggregate(id)
{
    private Event() : this(Guid.Empty, new Name()) { }
    
    public Name Name { get; private set; } = name;
    public void UpdateName(Name name) => Name = name;
}
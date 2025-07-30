using Domain.Primitives;

namespace Domain.Entities;

public class Event(Guid id, Name name) : Aggregate(id)
{
    public Name Name { get; private set; } = name;
}
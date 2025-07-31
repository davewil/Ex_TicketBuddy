using Domain.Primitives;
using Shared.Domain;

namespace Domain.Entities;

public class Event(Guid id, Name name) : Aggregate(id)
{
    public Name Name { get; private set; } = name;
    public void UpdateName(Name name) => Name = name;
}
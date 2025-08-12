using Domain.Primitives;
using Shared.Domain;

namespace Domain.Entities;

public class Event(Guid id, EventName name) : Aggregate(id)
{
    public EventName EventName { get; private set; } = name;
}
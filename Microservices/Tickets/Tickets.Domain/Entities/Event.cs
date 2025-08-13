using Shared.Domain;
using Tickets.Domain.Primitives;

namespace Tickets.Domain.Entities;

public class Event(Guid id, EventName name) : Aggregate(id)
{
    public EventName EventName { get; private set; } = name;
}
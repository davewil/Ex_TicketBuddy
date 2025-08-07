using Domain.Events.Primitives;

namespace Domain.Events.Entitites;

public class Event(Guid id, EventName eventName) : Aggregate(id)
{
    public EventName EventName { get; private set; } = eventName;
    public void UpdateName(EventName eventName) => EventName = eventName;
}
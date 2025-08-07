using System.ComponentModel.DataAnnotations;
using Domain.Events.Primitives;

namespace Domain.Events.Entities;

public class Event(Guid id, EventName eventName, DateTimeOffset date) : Aggregate(id)
{
    public EventName EventName { get; private set; } = eventName;
    public DateTimeOffset Date { get; private set; } = date;
    public void UpdateName(EventName eventName) => EventName = eventName;
    public void UpdateDate(DateTimeOffset date)
    {
        if (date < DateTimeOffset.Now) throw new ValidationException("Event date cannot be in the past");
        Date = date;
    }
}
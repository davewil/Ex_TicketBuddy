using System.ComponentModel.DataAnnotations;
using Domain.Events.Primitives;

namespace Domain.Events.Entities;

public class Event(Guid id, Name eventName, DateTimeOffset date) : Aggregate(id)
{
    public Name EventName { get; private set; } = eventName;
    public DateTimeOffset Date { get; private set; } = date;
    public void UpdateName(Name name) => EventName = name;
    public void UpdateDate(DateTimeOffset date)
    {
        if (date < DateTimeOffset.Now) throw new ValidationException("Event date cannot be in the past");
        Date = date;
    }
}
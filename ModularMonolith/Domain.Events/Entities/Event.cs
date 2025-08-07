using System.ComponentModel.DataAnnotations;
using Domain.Events.Primitives;

namespace Domain.Events.Entities;

public class Event : Aggregate
{
    public Event(Guid id, EventName eventName, DateTimeOffset date) : base(id)
    {
        if (date < DateTimeOffset.Now) throw new ValidationException("Event date cannot be in the past");
        
        EventName = eventName;
        Date = date;
    }
    
    public EventName EventName { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public void UpdateName(EventName eventName) => EventName = eventName;
    public void UpdateDate(DateTimeOffset date)
    {
        if (date < DateTimeOffset.Now) throw new ValidationException("Event date cannot be in the past");
        Date = date;
    }
}
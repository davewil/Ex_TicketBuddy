using System.ComponentModel.DataAnnotations;
using Domain.Events.Primitives;

namespace Domain.Events.Entities;

public class Event : Aggregate
{
    public Event(Guid id, EventName eventName, DateTimeOffset startDate, DateTimeOffset endDate, Venue venue) : base(id)
    {
        if (endDate < startDate) throw new ValidationException("End date cannot be before start date");
        EventName = eventName;
        StartDate = startDate;
        EndDate = endDate;
        Venue = venue;
    }
    
    public EventName EventName { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public Venue Venue { get; private set; }
    public void UpdateName(EventName eventName) => EventName = eventName;
    public void UpdateDate(DateTimeOffset date)
    {
        if (date < DateTimeOffset.Now) throw new ValidationException("Event date cannot be in the past");
        StartDate = date;
    }
    
    public void UpdateVenue(Venue venue) => Venue = venue;
}
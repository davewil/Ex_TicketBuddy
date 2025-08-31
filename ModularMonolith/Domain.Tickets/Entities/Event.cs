using System.ComponentModel.DataAnnotations;
using Domain.Events.Primitives;
using EventName = Domain.Tickets.Primitives.EventName;

namespace Domain.Tickets.Entities;

public class Event : Aggregate
{
    public Event(Guid id, EventName eventName, DateTimeOffset startDate, DateTimeOffset endDate, Domain.Events.Primitives.Venue venue, decimal price) : base(id)
    {
        if (endDate < startDate) throw new ValidationException("End date cannot be before start date");
        EventName = eventName;
        StartDate = startDate;
        EndDate = endDate;
        Venue = venue;
        Price = price;
    }
    
    public EventName EventName { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public Domain.Events.Primitives.Venue Venue { get; private set; }
    public decimal Price { get; private set; }
}
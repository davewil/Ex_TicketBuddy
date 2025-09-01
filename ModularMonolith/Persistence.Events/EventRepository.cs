using System.ComponentModel.DataAnnotations;
using Domain.Events.Primitives;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Event = Domain.Events.Entities.Event;

namespace Events.Persistence;

public class EventRepository(EventDbContext eventDbContext, IPublishEndpoint publishEndpoint)
{
    public async Task Add(Event theEvent)
    {
        var @event = await Get(theEvent.Id);
        if (@event is not null)
        {
            throw new ValidationException($"Event with id {theEvent.Id} already exists");
        }
        
        eventDbContext.Add(theEvent);

        await eventDbContext.SaveChangesAsync();
        
        await publishEndpoint.Publish(new Integration.Events.Messaging.Outbound.EventUpserted
        {
            Id = theEvent.Id, 
            EventName = theEvent.EventName,
            StartDate = theEvent.StartDate,
            EndDate = theEvent.EndDate,
            Venue = theEvent.Venue,
            Price = theEvent.Price
        });
    }
    
    public async Task Update(Guid id, EventName eventName, DateTimeOffset startDate, DateTimeOffset endDate, decimal price)
    {
        var @event = await Get(id);
        if (@event is null) throw new ValidationException($"Event with id {id} not found");
        @event.UpdateName(eventName);
        @event.UpdateDates(startDate, endDate);
        @event.UpdatePrice(price);
        eventDbContext.Update(@event);
        await eventDbContext.SaveChangesAsync();
        
        await publishEndpoint.Publish(new Integration.Events.Messaging.Outbound.EventUpserted
        {
            Id = @event.Id, 
            EventName = @event.EventName,
            StartDate = @event.StartDate,
            EndDate = @event.EndDate,
            Venue = @event.Venue,
            Price = @event.Price
        });
    }

    public async Task<Event?> Get(Guid id)
    {
        return await eventDbContext.Events.FindAsync(id);
    }

    public async Task<IList<Event>> GetAll()
    {
        return await eventDbContext.Events.Where(e => e.StartDate > DateTimeOffset.Now).ToListAsync();
    }
}
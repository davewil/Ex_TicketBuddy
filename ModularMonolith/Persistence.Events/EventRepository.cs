using MassTransit;
using Microsoft.EntityFrameworkCore;
using Event = Domain.Events.Entities.Event;

namespace Events.Persistence;

public class EventRepository(EventDbContext eventDbContext, IPublishEndpoint publishEndpoint)
{
    public async Task Save(Event theEvent)
    {
        var @event = await Get(theEvent.Id);
        if (@event is not null)
        {
            @event.UpdateName(theEvent.EventName);
            @event.UpdateDates(theEvent.StartDate, theEvent.EndDate);
            @event.UpdateVenue(theEvent.Venue);
            @event.UpdatePrice(theEvent.Price);
            eventDbContext.Update(@event);
        }
        else
        {
            eventDbContext.Add(theEvent);
        }

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

    public async Task<Event?> Get(Guid id)
    {
        return await eventDbContext.Events.FindAsync(id);
    }

    public async Task<IList<Event>> GetAll()
    {
        return await eventDbContext.Events.Where(e => e.StartDate > DateTimeOffset.Now).ToListAsync();
    }
}
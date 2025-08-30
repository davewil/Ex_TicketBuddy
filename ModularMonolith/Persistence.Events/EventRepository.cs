using MassTransit;
using Microsoft.EntityFrameworkCore;
using Event = Domain.Events.Entities.Event;

namespace Events.Persistence;

public class EventRepository(EventDbContext eventDbContext, IPublishEndpoint publishEndpoint)
{
    public async Task Save(Event theEvent)
    {
        if (await Get(theEvent.Id) != null)
        {
            eventDbContext.Update(theEvent);
            await eventDbContext.SaveChangesAsync();
        }
        else
        {
            eventDbContext.Add(theEvent);
            await eventDbContext.SaveChangesAsync();
        }
        
        await publishEndpoint.Publish(new Integration.Events.Messaging.EventUpserted
        {
            Id = theEvent.Id, 
            Name = theEvent.EventName,
            StartDate = theEvent.StartDate,
            EndDate = theEvent.EndDate,
            Venue = theEvent.Venue
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
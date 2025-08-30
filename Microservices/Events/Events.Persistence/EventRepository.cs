using Events.Domain.Messaging.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Event = Events.Domain.Entities.Event;

namespace Events.Persistence;

public class EventRepository(EventDbContext eventDbContext, IPublishEndpoint publishEndpoint)
{
    public async Task Save(Event theEvent)
    {
        if (await Get(theEvent.Id) != null)
        {
            eventDbContext.Update(theEvent);
            await publishEndpoint.Publish(new EventUpserted{ Id = theEvent.Id, Name = theEvent.EventName });
            await eventDbContext.SaveChangesAsync();
        }
        else
        {
            eventDbContext.Add(theEvent);
            await publishEndpoint.Publish(new EventUpserted{ Id = theEvent.Id, Name = theEvent.EventName });
            await eventDbContext.SaveChangesAsync();
        }
    }

    public async Task<Event?> Get(Guid id)
    {
        return await eventDbContext.Events.FindAsync(id);
    }

    public async Task<IList<Event>> GetAll()
    {
        return await eventDbContext.Events.ToListAsync();
    }
}
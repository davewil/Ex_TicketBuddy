using Events.Domain.Messaging.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class EventRepository(EventDbContext eventDbContext, IPublishEndpoint publishEndpoint)
{
    public async Task Save(Domain.Entities.Event theEvent)
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

    public async Task<Domain.Entities.Event?> Get(Guid id)
    {
        return await eventDbContext.Events.FindAsync(id);
    }

    public async Task<IList<Domain.Entities.Event>> GetAll()
    {
        return await eventDbContext.Events.ToListAsync();
    }
}
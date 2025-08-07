using Domain.Events.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Events.Persistence;

public class EventRepository(EventDbContext eventDbContext)
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
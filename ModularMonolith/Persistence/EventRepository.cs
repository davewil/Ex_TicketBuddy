using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class EventRepository(EventDbContext eventDbContext)
{
    public async Task Save(Domain.Entities.Event theEvent)
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

    public async Task<Domain.Entities.Event?> Get(Guid id)
    {
        return await eventDbContext.Events.FindAsync(id);
    }

    public async Task<IList<Domain.Entities.Event>> GetAll()
    {
        return await eventDbContext.Events.ToListAsync();
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class EventRepository(EventDbContext eventDbContext)
{
    public async Task Save(Event user)
    {
        if (await Get(user.Id) != null)
        {
            eventDbContext.Update(user);
            await eventDbContext.SaveChangesAsync();
        }
        else
        {
            eventDbContext.Add(user);
            await eventDbContext.SaveChangesAsync();
        }
    }

    public async Task Remove(Guid id)
    {
        var user = await Get(id);
        if (user is null) return;

        eventDbContext.Remove(user);
        await eventDbContext.SaveChangesAsync();
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
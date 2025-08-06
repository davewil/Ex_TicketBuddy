namespace Persistence.Events;

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

    public async Task Remove(Guid id)
    {
        var user = await Get(id);
        if (user is null) return;

        eventDbContext.Remove(user);
        await eventDbContext.SaveChangesAsync();
    }

    private async Task<Domain.Entities.Event?> Get(Guid id)
    {
        return await eventDbContext.Events.FindAsync(id);
    }
}
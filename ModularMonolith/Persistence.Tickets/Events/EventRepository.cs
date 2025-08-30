namespace Persistence.Tickets.Events;

public class EventRepository(TicketDbContext ticketDbContext)
{
    public async Task Save(Domain.Tickets.Entities.Event theEvent)
    {
        if (await Get(theEvent.Id) != null)
        {
            ticketDbContext.Update(theEvent);
            await ticketDbContext.SaveChangesAsync();
        }
        else
        {
            ticketDbContext.Add(theEvent);
            await ticketDbContext.SaveChangesAsync();
        }
    }

    private async Task<Domain.Tickets.Entities.Event?> Get(Guid id)
    {
        return await ticketDbContext.Events.FindAsync(id);
    }
}
using Domain.Tickets.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Tickets;

public class EventRepository(TicketDbContext ticketDbContext)
{
    public async Task Save(Event theEvent)
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

    public async Task<Venue> GetVenue(Domain.Events.Primitives.Venue venue)
    {
        return await ticketDbContext.Venues.FirstAsync(v => v.Id == venue);
    }

    public async Task<Event?> Get(Guid id)
    {
        return await ticketDbContext.Events.FindAsync(id);
    }
}
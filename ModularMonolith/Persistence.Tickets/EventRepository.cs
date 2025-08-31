using Domain.Tickets.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Persistence.Tickets.Messages;
using Event = Domain.Tickets.Entities.Event;

namespace Persistence.Tickets;

public class EventRepository(TicketDbContext ticketDbContext, IPublishEndpoint publisher)
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
        await publisher.Publish(new EventUpserted
        {
            Id = theEvent.Id,
            Venue = theEvent.Venue,
            Price = theEvent.Price
        });
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
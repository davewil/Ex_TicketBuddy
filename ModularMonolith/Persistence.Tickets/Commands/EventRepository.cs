using Domain.Tickets.Entities;
using Domain.Tickets.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Event = Domain.Tickets.Entities.Event;

namespace Persistence.Tickets.Commands;

public class EventRepository(TicketDbContext ticketDbContext, IPublishEndpoint publisher)
{
    public async Task Save(Event theEvent)
    {
        var @event = await Get(theEvent.Id);
        if (@event is not null)
        {
            @event.UpdateName(theEvent.EventName);
            @event.UpdateDates(theEvent.StartDate, theEvent.EndDate);
            @event.UpdateVenue(theEvent.Venue);
            @event.UpdatePrice(theEvent.Price);
            ticketDbContext.Update(@event);
        }
        else
        {
            ticketDbContext.Add(theEvent);
        }

        await ticketDbContext.SaveChangesAsync();
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

    private async Task<Event?> Get(Guid id)
    {
        return await ticketDbContext.Events.FindAsync(id);
    }
}
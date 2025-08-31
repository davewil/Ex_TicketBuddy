using System.ComponentModel.DataAnnotations;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Tickets;

public class TicketRepository(TicketDbContext context, IPublishEndpoint publishEndpoint)
{
    public async Task<IList<Domain.Tickets.Entities.Ticket>> GetTicketsForEvent(Guid eventId)
    {
        return await context.Tickets
            .Where(t => t.EventId == eventId && t.UserId == null)
            .ToListAsync();
    }    
    
    public async Task<IList<Domain.Tickets.Entities.Ticket>> GetTicketsForEventByUser(Guid eventId, Guid userId)
    {
        return await context.Tickets
            .Where(t => t.EventId == eventId && t.UserId == userId)
            .ToListAsync();
    }
    
    public async Task ReleaseTicketsForEvent(Guid eventId, int numberOfTickets, decimal pricePerTicket)
    {
        await CheckIfTicketsAlreadyReleased(eventId);

        for (uint i = 0; i < numberOfTickets; i++)
        {
            var ticket = new Domain.Tickets.Entities.Ticket(
                Guid.NewGuid(),
                eventId,
                pricePerTicket,
                i + 1);
            context.Tickets.Add(ticket);
        }
        await context.SaveChangesAsync();
        await publishEndpoint.Publish(new Integration.Tickets.Messaging.Outbound.TicketsReleased
        {
            EventId = eventId
        });
    }
    
    public async Task UpdateTicketPricesForEvent(Guid eventId, decimal newPricePerTicket)
    {
        var ticketsToUpdate = await context.Tickets
            .Where(t => t.EventId == eventId && t.UserId == null)
            .ToListAsync();
        
        foreach (var ticket in ticketsToUpdate)
        {
            ticket.UpdatePrice(newPricePerTicket);
            context.Tickets.Update(ticket);
        }
        
        await context.SaveChangesAsync();
    }

    private async Task CheckIfTicketsAlreadyReleased(Guid eventId)
    {
        var existingTickets = await context.Tickets
            .Where(t => t.EventId == eventId)
            .ToListAsync();
        if (existingTickets.Any()) throw new ValidationException("Tickets have already been released for this event");
    }

    public async Task PurchaseTickets(Guid eventId, Guid userId, IList<Guid> ticketIds)
    {
        var tickets = await context.Tickets
            .Where(t => ticketIds.Contains(t.Id) && t.EventId == eventId)
            .ToListAsync();
        
        foreach (var ticket in tickets)
        {
            ticket.Purchase(userId);
            context.Tickets.Update(ticket);
        }
        
        await context.SaveChangesAsync();
    }
}
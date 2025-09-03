using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Tickets.Commands;

public class TicketRepository(TicketDbContext context)
{
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
    
    public async Task<bool> AreTicketsReleasedForEvent(Guid eventId)
    {
        return await context.Tickets
            .Where(t => t.EventId == eventId)
            .AnyAsync();
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
        
        if (tickets.Count != ticketIds.Count) throw new ValidationException("One or more tickets do not exist");
        
        foreach (var ticket in tickets)
        {
            ticket.Purchase(userId);
            context.Tickets.Update(ticket);
        }
        
        await context.SaveChangesAsync();
    }
}
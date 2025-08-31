using Microsoft.EntityFrameworkCore;

namespace Persistence.Tickets;

public class TicketRepository(TicketDbContext context)
{
    public async Task<IList<Domain.Tickets.Entities.Ticket>> GetTicketsForEvent(Guid eventId)
    {
        return await context.Tickets
            .Where(t => t.EventId == eventId && t.UserId == null)
            .ToListAsync();
    }
    public async Task ReleaseTicketsForEvent(Guid eventId, int numberOfTickets, decimal pricePerTicket)
    {
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
    
    public async Task PurchaseTickets(Guid eventId, Guid userId, IList<Guid> ticketIds)
    {
        var tickets = await context.Tickets
            .Where(t => ticketIds.Contains(t.Id) && t.UserId == null && t.EventId == eventId)
            .ToListAsync();
        foreach (var ticket in tickets)
        {
            ticket.Purchase(userId);
            context.Tickets.Update(ticket);
        }
        await context.SaveChangesAsync();
    }
}
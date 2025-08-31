using Microsoft.EntityFrameworkCore;

namespace Persistence.Tickets;

public class TicketRepository(TicketDbContext context)
{
    public async Task<IList<Domain.Tickets.Entities.Ticket>> GetTicketsForEvent(Guid eventId)
    {
        return await context.Tickets
            .Where(t => t.EventId == eventId)
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
}
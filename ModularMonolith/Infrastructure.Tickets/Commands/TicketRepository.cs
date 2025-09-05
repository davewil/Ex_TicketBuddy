using Domain.Tickets.Contracts;
using Domain.Tickets.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tickets.Commands;

public class CommandTicketRepository(TicketDbContext context) : IAmATicketRepositoryForCommands
{
    public Task AddTickets(IEnumerable<Ticket> tickets)
    {
        context.Tickets.AddRange(tickets);
        return context.SaveChangesAsync();
    }

    public Task UpdateTickets(IEnumerable<Ticket> tickets)
    {
        context.Tickets.UpdateRange(tickets);
        return context.SaveChangesAsync();
    }

    public Task<IEnumerable<Ticket>> GetTicketsForEvent(Guid eventId)
    {
        return context.Tickets.Where(t => t.EventId == eventId).ToListAsync().ContinueWith(t => (IEnumerable<Ticket>)t.Result);
    }
}
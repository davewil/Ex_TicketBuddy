using Domain.Tickets.Entities;

namespace Domain.Tickets.Contracts;

public interface IAmATicketRepositoryForCommands
{
    public Task AddTickets(IEnumerable<Ticket> tickets);
    public Task UpdateTickets(IEnumerable<Ticket> tickets);
    public Task<IEnumerable<Ticket>> GetTicketsForEvent(Guid eventId);
}
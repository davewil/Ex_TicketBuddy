using Domain.Tickets.ReadModels;

namespace Domain.Tickets.Contracts;

public interface IAmATicketRepositoryForQueries
{
    public Task<IList<Ticket>> GetTicketsForEvent(Guid eventId);
    public Task<IList<Ticket>> GetTicketsForEventByUser(Guid eventId, Guid userId);
}
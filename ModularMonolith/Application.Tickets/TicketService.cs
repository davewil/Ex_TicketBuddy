using Persistence.Tickets;

namespace Application.Tickets;

public class TicketService(TicketRepository ticketRepository, EventRepository eventRepository)
{
    public async Task<IList<Domain.Tickets.Entities.Ticket>> GetTicketsForEvent(Guid eventId)
    {
        return await ticketRepository.GetTicketsForEvent(eventId);
    }

    public async Task<IList<Domain.Tickets.Entities.Ticket>> GetTicketsForEvent(Guid eventId, Guid userId)
    {
        return await ticketRepository.GetTicketsForEventByUser(eventId, userId);
    }

    public async Task PurchaseTickets(Guid eventId, Guid userId, IList<Guid> ticketIds)
    {
        await ticketRepository.PurchaseTickets(eventId, userId, ticketIds);
    }
}
using Persistence.Tickets;

namespace Application.Tickets;

public class TicketService(TicketRepository ticketRepository, EventRepository eventRepository)
{
    public async Task<IList<Domain.Tickets.Entities.Ticket>> GetTicketsForEvent(Guid eventId)
    {
        return await ticketRepository.GetTicketsForEvent(eventId);
    }
    public async Task ReleaseTicketsForEvent(Guid eventId, decimal pricePerTicket)
    {
        var theEvent = await eventRepository.Get(eventId);
        var venue = await eventRepository.GetVenue(theEvent!.Venue);
        await ticketRepository.ReleaseTicketsForEvent(theEvent.Id, venue.Capacity, pricePerTicket);
    }
    
    public async Task PurchaseTickets(Guid eventId, Guid userId, IList<Guid> ticketIds)
    {
        await ticketRepository.PurchaseTickets(eventId, userId, ticketIds);
    }
}
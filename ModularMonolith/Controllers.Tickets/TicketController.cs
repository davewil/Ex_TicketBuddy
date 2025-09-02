using Controllers.Tickets.Requests;
using Domain.Tickets.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence.Tickets;

namespace Controllers.Tickets;

[ApiController]
public class TicketController(TicketRepository TicketRepository) : ControllerBase
{
    [HttpGet(Routes.Tickets)]
    public async Task<IList<Ticket>> GetTickets([FromRoute] Guid id)
    {
        return await TicketRepository.GetTicketsForEvent(id);
    }
    
    [HttpPost(Routes.TicketsPurchase)]
    public async Task<ActionResult> PurchaseTickets([FromRoute] Guid id, [FromBody] TicketPurchasePayload payload)
    {
        await TicketRepository.PurchaseTickets(id, payload.userId, payload.ticketIds);
        return NoContent();
    }
    
    [HttpGet(Routes.TicketsPurchased)]
    public async Task<IList<Ticket>> GetTicketsForUser([FromRoute] Guid id, [FromRoute] Guid userId)
    {
        return await TicketRepository.GetTicketsForEventByUser(id, userId);
    }
}
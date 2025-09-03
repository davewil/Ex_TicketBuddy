using Controllers.Tickets.Requests;
using Domain.Tickets.ReadModels;
using Microsoft.AspNetCore.Mvc;
using Persistence.Tickets.Commands;
using Persistence.Tickets.Queries;

namespace Controllers.Tickets;

[ApiController]
public class TicketController(WriteOnlyTicketRepository WriteOnlyTicketRepository, ReadOnlyTicketRepository ReadOnlyTicketRepository) : ControllerBase
{
    [HttpGet(Routes.Tickets)]
    public async Task<IList<Ticket>> GetTickets([FromRoute] Guid id)
    {
        return await ReadOnlyTicketRepository.GetTicketsForEvent(id);
    }
    
    [HttpPost(Routes.TicketsPurchase)]
    public async Task<ActionResult> PurchaseTickets([FromRoute] Guid id, [FromBody] TicketPurchasePayload payload)
    {
        await WriteOnlyTicketRepository.PurchaseTickets(id, payload.userId, payload.ticketIds);
        return NoContent();
    }
    
    [HttpGet(Routes.TicketsPurchased)]
    public async Task<IList<Ticket>> GetTicketsForUser([FromRoute] Guid id, [FromRoute] Guid userId)
    {
        return await ReadOnlyTicketRepository.GetTicketsForEventByUser(id, userId);
    }
}
using Application.Tickets;
using Controllers.Tickets.Requests;
using Domain.Tickets.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Tickets;

[ApiController]
public class TicketController(TicketService TicketService) : ControllerBase
{
    [HttpGet(Routes.Tickets)]
    public async Task<IList<Ticket>> GetTickets([FromRoute] Guid id)
    {
        return await TicketService.GetTicketsForEvent(id);
    }
    
    [HttpPost(Routes.Tickets)]
    public async Task<ActionResult<Guid>> ReleaseTicketsForEvent([FromRoute] Guid id, [FromBody] TicketPayload payload)
    {
        await TicketService.ReleaseTicketsForEvent(id, payload.price);
        return Created(Routes.EventTickets(id), id);
    }
    
    [HttpPut(Routes.Tickets)]
    public async Task<ActionResult<Guid>> UpdateTicketsForEvent([FromRoute] Guid id, [FromBody] TicketPayload payload)
    {
        await TicketService.UpdateTicketPricesForEvent(id, payload.price);
        return Ok(id);
    }
    
    [HttpPost(Routes.TicketsPurchase)]
    public async Task<ActionResult> PurchaseTickets([FromRoute] Guid id, [FromBody] TicketPurchasePayload payload)
    {
        await TicketService.PurchaseTickets(id, payload.userId, payload.ticketIds);
        return Ok();
    }
    
    [HttpGet(Routes.TicketsPurchased)]
    public async Task<IList<Ticket>> GetTicketsForUser([FromRoute] Guid id, [FromRoute] Guid userId)
    {
        return await TicketService.GetTicketsForEvent(id, userId);
    }
}
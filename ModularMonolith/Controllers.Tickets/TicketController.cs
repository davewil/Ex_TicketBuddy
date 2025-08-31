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
    public async Task<ActionResult<Guid>> CreateEvent([FromRoute] Guid id, [FromBody] TicketPayload payload)
    {
        await TicketService.ReleaseTicketsForEvent(id, payload.price);
        return Created($"/{Routes.Tickets}/{id}", id);
    }
}
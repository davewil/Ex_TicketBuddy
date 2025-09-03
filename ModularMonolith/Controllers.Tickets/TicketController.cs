using Controllers.Tickets.Requests;
using Domain.Tickets.ReadModels;
using Microsoft.AspNetCore.Mvc;
using Persistence.Tickets.Commands;
using Persistence.Tickets.Queries;
using StackExchange.Redis;

namespace Controllers.Tickets;

[ApiController]
public class TicketController(
    WriteOnlyTicketRepository WriteOnlyTicketRepository,
    ReadOnlyTicketRepository ReadOnlyTicketRepository,
    IConnectionMultiplexer connectionMultiplexer)
    : ControllerBase
{
    [HttpGet(Routes.Tickets)]
    public async Task<IList<Ticket>> GetTickets([FromRoute] Guid id)
    {
        var tickets = await ReadOnlyTicketRepository.GetTicketsForEvent(id);
        DecorateTicketsWithReservationStatus(id, tickets);
        return tickets;
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
    
    [HttpPost(Routes.TicketsReservation)]
    public async Task<ActionResult> ReserveTickets([FromRoute] Guid id, [FromBody] TicketReservationPayload payload)
    {
        var db = connectionMultiplexer.GetDatabase();
        foreach (var ticketId in payload.ticketIds)
        {
            var reserved = await db.StringSetAsync(GetReservationKey(id, ticketId), payload.userId.ToString(), TimeSpan.FromMinutes(15), When.NotExists);
            if (!reserved) return Conflict($"Ticket {ticketId} is already reserved.");
        }
        return NoContent();
    }
    
    private static string GetReservationKey(Guid eventId, Guid ticketId) => $"event:{eventId}:ticket:{ticketId}:reservation";
    
    private void DecorateTicketsWithReservationStatus(Guid id, IList<Ticket> tickets)
    {
        var db = connectionMultiplexer.GetDatabase();
        foreach (var ticket in tickets)
        {
            var value = db.StringGet(GetReservationKey(id, ticket.Id));
            if (value.HasValue)
            {
                ticket.MarkTicketAsReserved();
            }
        }
    }
}
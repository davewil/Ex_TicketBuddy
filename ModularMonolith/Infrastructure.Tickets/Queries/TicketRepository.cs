using Domain.Tickets.Contracts;
using Domain.Tickets.ReadModels;

namespace Infrastructure.Tickets.Queries;

public class QueryTicketRepository(Database database) : IAmATicketRepositoryForQueries
{
    public async Task<IList<Ticket>> GetTicketsForEvent(Guid eventId)
    {
        return (await database.Query<Ticket>("""
                                              SELECT Id, EventId, Price, SeatNumber, CAST(CASE WHEN PurchasedAt IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS Purchased
                                              FROM Ticket.Tickets 
                                              WHERE EventId = @EventId
                                              """, new { EventId = eventId })).ToList();
    }
    
        
    public async Task<IList<Ticket>> GetTicketsForEventByUser(Guid eventId, Guid userId)
    {
        return (await database.Query<Ticket>("""
                                              SELECT Id, EventId, Price, SeatNumber, CAST(CASE WHEN PurchasedAt IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS Purchased
                                              FROM Ticket.Tickets 
                                              WHERE EventId = @EventId AND UserId = @UserId
                                              """, new { EventId = eventId, UserId = userId })).ToList();
    }
}
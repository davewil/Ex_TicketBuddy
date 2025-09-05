using Domain.Tickets.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tickets.Queries;

public class ReadOnlyTicketRepository(ReadOnlyTicketDbContext context)
{
    public async Task<IList<Ticket>> GetTicketsForEvent(Guid eventId)
    {
        return await context.Tickets
            .FromSqlInterpolated($"""
                                  SELECT Id, EventId, Price, SeatNumber, CAST(CASE WHEN PurchasedAt IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS Purchased
                                  FROM Ticket.Tickets 
                                  WHERE EventId = {eventId}
                                  """)
            .AsNoTracking()
            .ToListAsync();
    }
    
        
    public async Task<IList<Ticket>> GetTicketsForEventByUser(Guid eventId, Guid userId)
    {
        return await context.Tickets
            .FromSqlInterpolated($"""
                                  SELECT Id, EventId, Price, SeatNumber, CAST(CASE WHEN PurchasedAt IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS Purchased
                                  FROM Ticket.Tickets 
                                  WHERE EventId = {eventId} AND UserId = {userId}
                                  """)
            .AsNoTracking()
            .ToListAsync();
    }
}
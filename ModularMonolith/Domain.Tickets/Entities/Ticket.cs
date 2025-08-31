namespace Domain.Tickets.Entities;

public class Ticket(Guid Id, Guid eventId, decimal price, uint SeatNumber) : Aggregate(Id)
{
    public Guid EventId { get; private set; } = eventId;
    public decimal Price { get; private set; } = price;
    public uint SeatNumber { get; private set; } = SeatNumber;
}
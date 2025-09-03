namespace Domain.Tickets.ReadModels;

public class Ticket(Guid Id, Guid eventId, decimal price, uint SeatNumber, bool Purchased)
{
    public Guid Id { get; } = Id;
    public Guid EventId { get; } = eventId;
    public decimal Price { get; } = price;
    public uint SeatNumber { get; } = SeatNumber;
    public bool Purchased { get; } = Purchased;
}
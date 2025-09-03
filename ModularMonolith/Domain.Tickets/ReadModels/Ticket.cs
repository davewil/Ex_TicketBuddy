using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Tickets.ReadModels;

public class Ticket(Guid Id, Guid eventId, decimal price, uint SeatNumber, bool Purchased)
{
    public Guid Id { get; } = Id;
    public Guid EventId { get; private set; } = eventId;
    public decimal Price { get; private set; } = price;
    public uint SeatNumber { get; private set; } = SeatNumber;
    public bool Purchased { get; private set; } = Purchased;
    [NotMapped]
    public bool Reserved { get; private set; }
    public void MarkTicketAsReserved() => Reserved = true;
}
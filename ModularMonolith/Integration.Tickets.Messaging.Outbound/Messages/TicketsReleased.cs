namespace Integration.Tickets.Messaging.Outbound;

public record TicketsReleased
{
    public Guid EventId { get; init; }
    public int NumberOfTickets { get; init; }
}
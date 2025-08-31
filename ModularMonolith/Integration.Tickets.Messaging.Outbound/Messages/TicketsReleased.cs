namespace Integration.Tickets.Messaging.Outbound;

public record TicketsReleased
{
    public Guid EventId { get; init; }
}
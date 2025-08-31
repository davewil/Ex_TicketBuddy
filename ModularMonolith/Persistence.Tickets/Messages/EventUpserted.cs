using Domain.Events.Primitives;

namespace Persistence.Tickets.Messages;

public record EventUpserted
{
    public Guid Id { get; init; }
    public Venue Venue { get; init; }
    public decimal Price { get; init; }
}
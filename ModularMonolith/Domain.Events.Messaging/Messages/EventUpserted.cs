using Domain.Events.Primitives;

namespace Domain.Events.Messaging.Messages;

public record EventUpserted
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public Venue Venue { get; init; }
}
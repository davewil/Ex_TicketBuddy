namespace Events.Domain.Messaging.Messages;

public record EventUpserted
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}
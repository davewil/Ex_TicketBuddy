namespace Events.Integration.Messaging.Outbound.Messages;

public record EventUpserted
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
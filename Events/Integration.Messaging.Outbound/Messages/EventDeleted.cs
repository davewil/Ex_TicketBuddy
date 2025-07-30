namespace Events.Integration.Messaging.Outbound.Messages;

public record EventDeleted
{
    public Guid Id { get; init; }
}
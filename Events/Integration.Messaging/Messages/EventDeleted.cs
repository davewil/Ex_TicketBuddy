namespace Integration.Messaging.Messages;

public record EventDeleted
{
    public Guid Id { get; init; }
}
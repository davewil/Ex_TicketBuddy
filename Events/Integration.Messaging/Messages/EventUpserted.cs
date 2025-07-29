namespace Integration.Messaging.Messages;

public record EventUpserted
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}

public record EventDeleted
{
    public Guid Id { get; init; }
}
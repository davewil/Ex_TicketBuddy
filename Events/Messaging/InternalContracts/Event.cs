namespace Messaging.InternalContracts;

public record InternalEventUpserted
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}

public record InternalEventDeleted
{
    public Guid Id { get; init; }
}
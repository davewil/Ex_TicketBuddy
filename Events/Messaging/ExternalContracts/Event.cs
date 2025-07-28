namespace Messaging.ExternalContracts;

public record Event
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}

public record EventDeleted
{
    public Guid Id { get; init; }
}
namespace Messaging.Contracts;

public record Event
{
    public string Name { get; init; } = string.Empty;
}
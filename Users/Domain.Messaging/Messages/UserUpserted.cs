namespace Users.Domain.Messaging.Messages;

public record UserUpserted
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
}
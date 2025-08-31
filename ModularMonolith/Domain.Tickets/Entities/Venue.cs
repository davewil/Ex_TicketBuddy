namespace Domain.Tickets.Entities;

public class Venue(Events.Primitives.Venue Id, string Name, int Capacity)
{
    public Events.Primitives.Venue Id { get; } = Id;
    public string Name { get; init; } = Name;
    public int Capacity { get; init; } = Capacity;
}
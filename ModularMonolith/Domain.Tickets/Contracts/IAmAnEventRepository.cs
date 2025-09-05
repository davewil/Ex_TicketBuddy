using Domain.Tickets.Entities;

namespace Domain.Tickets.Contracts;

public interface IAmAnEventRepository
{
    public Task Save(Event theEvent);
    public Task<Venue> GetVenue(Domain.Events.Primitives.Venue venue);
}
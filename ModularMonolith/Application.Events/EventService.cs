using Domain.Events.Entities;
using Domain.Events.Primitives;
using Events.Persistence;

namespace Application.Events;

public class EventService(EventRepository repository)
{
    public async Task<Event?> Get(Guid id)
    {
        return await repository.Get(id);
    }

    public async Task<IList<Event>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<Guid> Add(EventName eventName, DateTimeOffset startDate, DateTimeOffset endDate, Venue venue, decimal price)
    {
        var id = Guid.NewGuid();
        var @event = new Event(id, eventName, startDate, endDate, venue, price);
        await repository.Save(@event);
        return id;
    }

    public async Task Update(Guid id, EventName eventName, DateTimeOffset startDate, DateTimeOffset endDate, Venue venue, decimal price)
    {
        var @event = await repository.Get(id);
        if (@event is null) return;
        
        @event.UpdateName(eventName);
        @event.UpdateDates(startDate, endDate);
        @event.UpdateVenue(venue);
        @event.UpdatePrice(price);
        await repository.Save(@event);
    }
}

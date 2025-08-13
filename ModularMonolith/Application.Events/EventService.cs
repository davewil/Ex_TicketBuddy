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

    public async Task<Guid> Add(EventName eventName, DateTimeOffset date, Venue venue)
    {
        var id = Guid.NewGuid();
        var @event = new Event(id, eventName, date, venue);
        await repository.Save(@event);
        return id;
    }

    public async Task Update(Guid id, EventName eventName, DateTimeOffset date, Venue venue)
    {
        var @event = await repository.Get(id);
        if (@event is null) return;
        
        @event.UpdateName(eventName);
        @event.UpdateDate(date);
        @event.UpdateVenue(venue);
        await repository.Save(@event);
    }
}

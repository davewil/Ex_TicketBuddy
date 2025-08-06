using Domain.Entities;
using Persistence;

namespace Application;

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

    public async Task<Guid> Add(string name)
    {
        var id = Guid.NewGuid();
        var user = new Event(id, name);
        await repository.Save(user);
        return id;
    }

    public async Task Update(Guid id, string name)
    {
        var user = await repository.Get(id);
        if (user is null) return;
        
        user.UpdateName(name);
        await repository.Save(user);
    }

    public async Task Remove(Guid id)
    {
        await repository.Remove(id);
    }
}

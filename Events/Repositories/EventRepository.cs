using Domain.Entities;
using Persistence;

namespace Repositories;

public class EventRepository(Db db)
{
    public async Task Save(Event user)
    {
        if (await Get(user.Id) != null)
        {
            await db.ExecuteAsync("UPDATE [Event].[Events] SET Name = @Name WHERE Id = @Id",
                new { user.Id, user.Name });
        }
        else
        {
            await db.ExecuteAsync("INSERT INTO [Event].[Events] (Id, Name) VALUES (@Id, @Name)",
                new { user.Id, user.Name });
        }
    }

    public async Task Remove(Guid id)
    {
        await db.ExecuteAsync("DELETE FROM [Event].[Events] WHERE Id = @Id", new { Id = id });
    }

    public async Task<Event?> Get(Guid id)
    {
        return (await db.QueryAsync<Event>("SELECT Id, Name FROM [Event].[Events] WHERE Id = @Id",
            new { Id = id })).FirstOrDefault();
    }

    public async Task<IList<Event>> GetAll()
    {
        return (await db.QueryAsync<Event>("SELECT Id, Name FROM [Event].[Events]")).ToList();
    }
}
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Messaging.Messages;

namespace Persistence;

public class UserRepository(UserDbContext userDbContext, IPublishEndpoint publishEndpoint)
{
    public async Task Save(User theUser)
    {
        if (await Get(theUser.Id) != null)
        {
            userDbContext.Update(theUser);
            await publishEndpoint.Publish(new UserUpserted{ Id = theUser.Id, FullName = theUser.FullName, Email = theUser.Email });
            await userDbContext.SaveChangesAsync();
        }
        else
        {
            userDbContext.Add(theUser);
            await publishEndpoint.Publish(new UserUpserted{ Id = theUser.Id, FullName = theUser.FullName, Email = theUser.Email });
            await userDbContext.SaveChangesAsync();
        }
    }

    public async Task<User?> Get(Guid id)
    {
        return await userDbContext.Users.FindAsync(id);
    }

    public async Task<IList<User>> GetAll()
    {
        return await userDbContext.Users.ToListAsync();
    }
}
using Domain.Users.Contracts;
using Domain.Users.Entities;
using Integration.Users.Messaging.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users.Persistence;

public class UserRepository(UserDbContext userDbContext, IPublishEndpoint publishEndpoint) : IAmAUserRepository
{
    public async Task Add(User theUser)
    {
        userDbContext.Add(theUser);
        await userDbContext.SaveChangesAsync();

        await publishEndpoint.Publish(new UserUpserted
        {
            Id = theUser.Id,
            FullName = theUser.FullName,
            Email = theUser.Email
        });
    }
    
    public async Task Update(User theUser)
    {
        userDbContext.Update(theUser);
        await userDbContext.SaveChangesAsync();

        await publishEndpoint.Publish(new UserUpserted
        {
            Id = theUser.Id,
            FullName = theUser.FullName,
            Email = theUser.Email
        });
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
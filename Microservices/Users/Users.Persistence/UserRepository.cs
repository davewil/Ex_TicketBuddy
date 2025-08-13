using MassTransit;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Messaging.Messages;
using System;
using System.ComponentModel.DataAnnotations;

namespace Users.Persistence;

public class UserRepository(UserDbContext userDbContext, IPublishEndpoint publishEndpoint)
{
    public async Task Save(User theUser)
    {
        if (await IsEmailAlreadyUsedByOtherUser(theUser.Id, theUser.Email)) throw new ValidationException("Email already exists");

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

    private async Task<bool> IsEmailAlreadyUsedByOtherUser(Guid userId, string email)
    {
        return await userDbContext.Users.AnyAsync(u => u.Email == email && u.Id != userId);
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
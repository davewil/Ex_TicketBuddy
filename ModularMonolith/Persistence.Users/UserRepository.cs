using System.ComponentModel.DataAnnotations;
using Domain.Users.Entities;
using Domain.Users.Primitives;
using Integration.Users.Messaging.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Users.Persistence;

public class UserRepository(UserDbContext userDbContext, IPublishEndpoint publishEndpoint)
{
    public async Task Add(User theUser)
    {
        if (await IsEmailAlreadyUsedByOtherUser(theUser.Id, theUser.Email)) throw new ValidationException("Email already exists");
        var existingUser = await Get(theUser.Id);

        if (existingUser is not null)
        {
            throw new ValidationException("User already exists");
        }
        
        userDbContext.Add(theUser);
        await userDbContext.SaveChangesAsync();

        await publishEndpoint.Publish(new UserUpserted
        {
            Id = theUser.Id,
            FullName = theUser.FullName,
            Email = theUser.Email
        });
    }
    
    public async Task Update(Guid id, FullName fullName, Email email)
    {
        if (await IsEmailAlreadyUsedByOtherUser(id, email)) throw new ValidationException("Email already exists");
        var existingUser = await Get(id);

        if (existingUser is null) throw new ValidationException("User does not exist");

        existingUser.UpdateName(fullName);
        existingUser.UpdateEmail(email);
        userDbContext.Update(existingUser);

        await userDbContext.SaveChangesAsync();

        await publishEndpoint.Publish(new UserUpserted
        {
            Id = existingUser.Id,
            FullName = existingUser.FullName,
            Email = existingUser.Email
        });
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
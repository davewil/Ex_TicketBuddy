using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Persistence;

public class UserRepository(UserDbContext userDbContext)
{
    public async Task Save(User theUser)
    {
        if (await IsEmailAlreadyUsedByOtherUser(theUser.Id, theUser.Email)) throw new ValidationException("Email already exists");

        if (await Get(theUser.Id) != null)
        {
            userDbContext.Update(theUser);
            await userDbContext.SaveChangesAsync();
        }
        else
        {
            userDbContext.Add(theUser);
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
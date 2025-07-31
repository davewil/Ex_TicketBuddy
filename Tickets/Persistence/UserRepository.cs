using Domain.Entities;

namespace Persistence;

public class UserRepository(UserDbContext userDbContext)
{
    public async Task Save(User theUser)
    {
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

    public async Task<User?> Get(Guid id)
    {
        return await userDbContext.Users.FindAsync(id);
    }
}
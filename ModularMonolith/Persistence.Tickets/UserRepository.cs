using Domain.Tickets.Entities;

namespace Persistence.Tickets;

public class UserRepository(TicketDbContext ticketDbContext)
{
    public async Task Save(User theUser)
    {
        var existingUser = await Get(theUser.Id);
        if (existingUser is not null)
        {
            existingUser.UpdateName(theUser.FullName);
            existingUser.UpdateEmail(theUser.Email);
            ticketDbContext.Update(existingUser);
        }
        else
        {
            ticketDbContext.Add(theUser);
        }

        await ticketDbContext.SaveChangesAsync();
    }

    private async Task<User?> Get(Guid id)
    {
        return await ticketDbContext.Users.FindAsync(id);
    }
}
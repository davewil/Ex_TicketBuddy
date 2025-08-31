using Domain.Tickets.Entities;

namespace Persistence.Tickets;

public class UserRepository(TicketDbContext ticketDbContext)
{
    public async Task Save(User theUser)
    {
        if (await Get(theUser.Id) != null)
        {
            ticketDbContext.Update(theUser);
            await ticketDbContext.SaveChangesAsync();
        }
        else
        {
            ticketDbContext.Add(theUser);
            await ticketDbContext.SaveChangesAsync();
        }
    }

    private async Task<User?> Get(Guid id)
    {
        return await ticketDbContext.Users.FindAsync(id);
    }
}
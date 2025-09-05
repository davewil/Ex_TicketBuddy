using Domain.Tickets.Entities;

namespace Domain.Tickets.Contracts;

public interface IAmAUserRepository
{
    public Task Save(User user);
}
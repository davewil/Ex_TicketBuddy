using Domain.Users.Entities;
using Domain.Users.Primitives;
using Users.Persistence;

namespace Application;

public class UserService(UserRepository repository)
{
    public async Task<User?> Get(Guid id)
    {
        return await repository.Get(id);
    }

    public async Task<IList<User>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<Guid> Add(FullName name, Email email, UserType userType)
    {
        var id = Guid.NewGuid();
        var user = new User(id, name, email, userType);
        await repository.Save(user);
        return id;
    }

    public async Task Update(Guid id, FullName name, Email email)
    {
        var user = await repository.Get(id);
        if (user is null) return;
        
        user.UpdateName(name);
        user.UpdateEmail(email);
        await repository.Save(user);
    }
}

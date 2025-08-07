using Domain.Users.Primitives;

namespace Domain.Users.Entities
{
    public class User(Guid id, FullName fullName, Email email) : Aggregate(id)
    {
        public FullName FullName { get; private set; } = fullName;
        public Email Email { get; private set; } = email;
        
        public void UpdateName(FullName newFullName)
        {
            FullName = newFullName;
        }
        
        public void UpdateEmail(Email newEmail)
        {
            Email = newEmail;
        }
    }
}
    
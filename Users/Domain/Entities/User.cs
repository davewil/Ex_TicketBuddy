using Shared.Domain;
using Users.Domain.Primitives;

namespace Users.Domain.Entities
{
    public class User(Guid id, Name fullName, Email email) : Aggregate(id)
    {
        public Name FullName { get; private set; } = fullName;
        public Email Email { get; private set; } = email;
        
        public void UpdateName(Name newName)
        {
            FullName = newName;
        }
        
        public void UpdateEmail(Email newEmail)
        {
            Email = newEmail;
        }
    }
}
    
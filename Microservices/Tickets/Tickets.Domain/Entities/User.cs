using Shared.Domain;
using Tickets.Domain.Primitives;

namespace Tickets.Domain.Entities
{
    public class User(Guid id, Name fullName, Email email) : Aggregate(id)
    {
        public Name FullName { get; private set; } = fullName;
        public Email Email { get; private set; } = email;
    }
}
    
using Tickets.Domain.Entities;
using MassTransit;
using Tickets.Persistence.Users;
using Users.Integration.Messaging.Outbound.Messages;

namespace Tickets.Integration.Messaging.Inbound.Consumers
{
    public class UserConsumer(UserRepository userRepository) : IConsumer<UserUpserted>
    {
        public async Task Consume(ConsumeContext<UserUpserted> context)
        {
            await userRepository.Save(new User(context.Message.Id, context.Message.FullName, context.Message.Email));
        }
    }
}
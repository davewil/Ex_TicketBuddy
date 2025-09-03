using Domain.Tickets.Entities;
using Integration.Users.Messaging.Outbound.Messages;
using MassTransit;
using Persistence.Tickets;
using Persistence.Tickets.Commands;

namespace Integration.Tickets.Messaging.Inbound.Consumers
{
    public class UserConsumer(UserRepository userRepository) : IConsumer<UserUpserted>
    {
        public async Task Consume(ConsumeContext<UserUpserted> context)
        {
            await userRepository.Save(new User(context.Message.Id, context.Message.FullName, context.Message.Email));
        }
    }
}
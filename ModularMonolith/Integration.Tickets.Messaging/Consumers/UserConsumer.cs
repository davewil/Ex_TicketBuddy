using Domain.Tickets.Entities;
using Integration.Users.Messaging.Messages;
using MassTransit;
using Persistence.Tickets;

namespace Integration.Tickets.Messaging.Consumers
{
    public class UserConsumer(UserRepository userRepository) : IConsumer<UserUpserted>
    {
        public async Task Consume(ConsumeContext<UserUpserted> context)
        {
            await userRepository.Save(new User(context.Message.Id, context.Message.FullName, context.Message.Email));
        }
    }
}
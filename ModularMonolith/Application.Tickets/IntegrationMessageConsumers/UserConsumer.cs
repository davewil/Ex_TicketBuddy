using Domain.Tickets.Entities;
using Infrastructure.Tickets.Commands;
using Integration.Users.Messaging.Messages;
using MassTransit;

namespace Application.Tickets.IntegrationMessageConsumers
{
    public class UserConsumer(UserRepository userRepository) : IConsumer<UserUpserted>
    {
        public async Task Consume(ConsumeContext<UserUpserted> context)
        {
            await userRepository.Save(new User(context.Message.Id, context.Message.FullName, context.Message.Email));
        }
    }
}
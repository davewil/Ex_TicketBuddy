using Domain.Users.Messaging.Messages;
using MassTransit;

namespace Domain.Users.Messaging.Consumers
{
    public class UserConsumer : IConsumer<UserUpserted>
    {
        public async Task Consume(ConsumeContext<UserUpserted> context)
        {
           await context.Publish(new Integration.Users.Messaging.Messages.UserUpserted{ Id = context.Message.Id, FullName = context.Message.FullName, Email = context.Message.Email });
        }
    }
}
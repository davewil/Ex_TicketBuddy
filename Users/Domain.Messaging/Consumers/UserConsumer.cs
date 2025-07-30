using MassTransit;
using Users.Domain.Messaging.Messages;

namespace Users.Domain.Messaging.Consumers
{
    public class UserConsumer : IConsumer<UserUpserted>
    {
        public async Task Consume(ConsumeContext<UserUpserted> context)
        {
           await context.Publish(new Events.Integration.Messaging.Outbound.Messages.UserUpserted{ Id = context.Message.Id, FullName = context.Message.FullName, Email = context.Message.Email });
        }
    }
}
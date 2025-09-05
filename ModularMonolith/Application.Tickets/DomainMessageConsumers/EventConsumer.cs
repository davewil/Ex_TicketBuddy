using Domain.Tickets.Messages;
using MassTransit;
using Persistence.Tickets.Commands;

namespace Application.Tickets.DomainMessageConsumers
{
    public class EventConsumer(WriteOnlyTicketRepository writeOnlyTicketRepository, EventRepository eventRepository) : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
           if (await writeOnlyTicketRepository.AreTicketsReleasedForEvent(context.Message.Id))
           {
               await writeOnlyTicketRepository.UpdateTicketPricesForEvent(context.Message.Id, context.Message.Price);
               return;
           }
           var venue = await eventRepository.GetVenue(context.Message.Venue);
           await writeOnlyTicketRepository.ReleaseTicketsForEvent(context.Message.Id, venue.Capacity, context.Message.Price);
        }
    }
}
using Domain.Tickets.Contracts;
using Domain.Tickets.Entities;
using Domain.Tickets.Messages;
using MassTransit;

namespace Domain.Tickets.DomainMessageConsumers
{
    public class EventConsumer(IAmATicketRepositoryForCommands commandTicketRepository, IAmAnEventRepository eventRepository) : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
           var existingTickets = (await commandTicketRepository.GetTicketsForEvent(context.Message.Id)).ToArray();
           if (existingTickets.Any())
           {
               await UpdateExistingTicketsThatAreNotPurchased(context, existingTickets);
               return;
           }
           await ReleaseNewTickets(context);
        }

        private async Task ReleaseNewTickets(ConsumeContext<EventUpserted> context)
        {
            var venue = await eventRepository.GetVenue(context.Message.Venue);
            var newTickets = new List<Ticket>();
            for (uint i = 0; i < venue.Capacity; i++)
            {
                var ticket = new Ticket(
                    Guid.NewGuid(),
                    context.Message.Id,
                    context.Message.Price,
                    i + 1); 
                newTickets.Add(ticket);
            }
            await commandTicketRepository.AddTickets(newTickets);
        }

        private async Task UpdateExistingTicketsThatAreNotPurchased(ConsumeContext<EventUpserted> context, Ticket[] existingTickets)
        {
            var existingTicketsNotPurchased = existingTickets.Where(t => t.UserId == null).ToList();
            foreach (var ticket in existingTicketsNotPurchased)
            {
                ticket.UpdatePrice(context.Message.Price);
            }
            await commandTicketRepository.UpdateTickets(existingTicketsNotPurchased);
        }
    }
}
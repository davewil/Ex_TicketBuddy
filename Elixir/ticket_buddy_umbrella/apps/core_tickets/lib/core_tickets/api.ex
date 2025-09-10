defmodule CoreTickets.Domain do
  use Ash.Domain,
    extensions: [AshJsonApi.Domain]

  resources do
    resource CoreTickets.TicketResource do
      # Code interface definitions (per-resource form for current Ash version)
      define :create_ticket, action: :create
      define :get_ticket, action: :read, get_by: [:id]
      define :update_ticket, action: :update
      define :process_reservation, action: :process_reservation
  define :list_confirmed_tickets, action: :list_confirmed
    end

    resource CoreTickets.OutboxMessageResource do
      define :enqueue_outbox_message, action: :enqueue
      define :list_outbox_messages, action: :read
      define :mark_outbox_published, action: :mark_published
    end
  end
end
